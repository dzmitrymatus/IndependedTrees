using AutoMapper;
using IndependedTrees.BLL.Exceptions;
using IndependedTrees.BLL.Models.Trees;
using IndependedTrees.DAL.Models.Tree;
using IndependedTrees.DAL.Repository;

namespace IndependedTrees.BLL.Services.Trees
{
    public class IndependedTreesService : IIndependedTreesService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<TreeNode> _repository;

        public IndependedTreesService(IMapper mapper,
            IRepository<TreeNode> repository)
        {
            this._mapper = mapper;
            this._repository = repository;
        }

        public async Task<TreeNodeModel> GetTreeAsync(string treeName)
        {
            var treeHead = await GetTreeHeadAsync(treeName)
                .ContinueWith(task => this._mapper.Map<TreeNodeModel>(task.Result));

            var treeNodes = await GetTreeNodesAsync(treeHead.TreeId)
                .ContinueWith(task => this._mapper.Map<IEnumerable<TreeNodeModel>>(task.Result));

            treeHead.Childrens = GenerateTree(treeNodes, treeHead.Id);

            return treeHead;
        }

        public async Task<IEnumerable<TreeNodeModel>> GetTreesAsync()
        {
            var treeHeads = await this._repository
                .GetAllAsync(x => x.Where(item => item.ParentId == null))
                .ContinueWith(task => this._mapper.Map<IEnumerable<TreeNodeModel>>(task.Result));

            foreach(var treeHead in treeHeads)
            {
                var treeNodes = await GetTreeNodesAsync(treeHead.TreeId)
                    .ContinueWith(task => this._mapper.Map<IEnumerable<TreeNodeModel>>(task.Result));

                treeHead.Childrens = GenerateTree(treeNodes, treeHead.Id);
            }

            return treeHeads;
        }

        public async Task CreateTreeAsync(string treeName)
        {
            var trees = await this._repository.GetAllAsync(x => x.Where(item => item.ParentId == null));
            if(trees.Any(x => x.Name.Equals(treeName)))
                throw new SecureException($"Tree with name '{treeName}' already exist.");

            var maxTreeId = trees.Max(x => x.TreeId);

            var node = new TreeNode()
            {
                Name = treeName,
                ParentId = null,
                TreeId = maxTreeId.HasValue? maxTreeId + 1 : 0
            };

            await this._repository.InsertAsync(node);
        }

        public async Task CreateTreeNodeAsync(string treeName, int parentNodeId, string nodeName)
        {
            var treeHead = await GetTreeHeadAsync(treeName);
            var treeNodes = await GetTreeNodesAsync(treeHead.TreeId);

            if(treeNodes.Where(x => x.Id == parentNodeId).Any() == false)
                throw new SecureException($"Parent TreeNode with id '{parentNodeId}' doesn't exist.");

            if (treeNodes.Where(x => x.ParentId == parentNodeId).Any(x => x.Name.Equals(nodeName)))
                throw new SecureException($"TreeNode with name '{nodeName}' already exist.");

            var node = new TreeNode()
            {
                Name = nodeName,
                ParentId = parentNodeId,
                TreeId = treeHead.TreeId
            };

            await this._repository.InsertAsync(node);
        }

        public async Task RenameTreeNodeAsync(string treeName, int nodeId, string newNodeName)
        {
            var treeHead = await GetTreeHeadAsync(treeName);
            var node = await GetTreeNodeAsync(treeHead.Id, nodeId);            
            var siblingNodes = await this._repository.GetAllAsync(
                x => x.Where(item => item.ParentId == node.ParentId));

            if (siblingNodes.Any(item => item.Name.Equals(newNodeName))) 
                throw new SecureException($"TreeNode with name '{newNodeName}' already exist.");

            node.Name = newNodeName;

            await this._repository.UpdateAsync(node);
        }

        public async Task DeleteTreeNodeAsync(string treeName, int nodeId)
        {
            var treeHead = await GetTreeHeadAsync(treeName);

            var node = await GetTreeNodeAsync(treeHead.TreeId, nodeId);
                
            var childNodes = await this._repository.GetAllAsync(
                x => x.Where(item => item.ParentId == node.Id));

            if (childNodes.Any())
                throw new SecureException($"You have to delete all children nodes first");

            await this._repository.DeleteAsync(node);
        }

        private async Task<TreeNode> GetTreeHeadAsync(string treeName)
        {
            var treeHead = await this._repository
                .GetAllAsync(x => x.Where(
                    item => item.ParentId == null
                            && item.Name.Equals(treeName)))
                .ContinueWith(task => task.Result.FirstOrDefault());

            if (treeHead == null) throw new SecureException($"Tree with name '{treeName}' doesn't exist.");

            return treeHead;
        }

        private async Task<TreeNode> GetTreeNodeAsync(int? treeId, int nodeId)
        {
            var node = await this._repository.GetAllAsync(
                x => x.Where(item => item.TreeId == treeId && item.Id == nodeId))
                .ContinueWith(task => task.Result.FirstOrDefault());

            if (node == null) throw new SecureException($"TreeNode with id '{nodeId}' doesn't exist.");

            return node;
        }

        private async Task<IEnumerable<TreeNode>> GetTreeNodesAsync(int? treeId)
        {
            return await this._repository
                .GetAllAsync(x => x.Where(item => item.TreeId == treeId));
        }

        private IEnumerable<TreeNodeModel> GenerateTree(IEnumerable<TreeNodeModel> nodes, int? parentId)
        {
            var childNodes = nodes.Where(x => x.ParentId == parentId);
            foreach(var childNode in childNodes)
            {
                childNode.Childrens = GenerateTree(nodes, childNode.Id);
            }

            return childNodes;
        }
    }
}
