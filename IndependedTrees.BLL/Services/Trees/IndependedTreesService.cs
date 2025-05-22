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
            var treeHead = await GetTreeHead(treeName)
                .ContinueWith(task => this._mapper.Map<TreeNodeModel>(task.Result));

            var treeNodes = await GetTreeNodes(treeHead.TreeId)
                .ContinueWith(task => this._mapper.Map<IEnumerable<TreeNodeModel>>(task.Result));

            treeHead.Childrens = GenerateTree(treeNodes, treeHead.Id);

            return treeHead;
        }

        public async Task CreateTreeNodeAsync(string treeName, int parentNodeId, string nodeName)
        {
            var treeHead = await GetTreeHead(treeName);
            var treeNodes = await GetTreeNodes(treeHead.TreeId);

            if(treeNodes.Where(x => x.Id == parentNodeId).Any() == false)
                throw new SecureException($"Parent TreeNode with id '{parentNodeId}' doesn't exist.");

            if (treeNodes.Where(x => x.ParentId == parentNodeId).Any(x => x.Name.Equals(nodeName)))
                throw new SecureException($"TreeNode with name '{nodeName}' already exist.");

            var node = new TreeNode()
            {
                Name = nodeName,
                ParentId = parentNodeId,
                TreeId = treeHead.Id
            };

            await this._repository.InsertAsync(node);
        }

        private async Task<TreeNode> GetTreeHead(string treeName)
        {
            var treeHead = await this._repository
                .GetAllAsync(x => x.Where(
                    item => item.ParentId == null
                            && item.Name.Equals(treeName)))
                .ContinueWith(task => task.Result.FirstOrDefault());

            if (treeHead == null) throw new SecureException($"Tree with name '{treeName}' doesn't exist.");

            return treeHead;
        }

        private async Task<IEnumerable<TreeNode>> GetTreeNodes(int? treeId)
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
