using IndependedTrees.BLL.Models.Trees;

namespace IndependedTrees.BLL.Services.Trees
{
    public interface IIndependedTreesService
    {
        Task<TreeNodeModel> GetTreeAsync(string treeName);
        Task<IEnumerable<TreeNodeModel>> GetTreesAsync();
        Task CreateTreeAsync(string treeName);
        Task CreateTreeNodeAsync(string treeName, int parentNodeId, string nodeName);
        Task RenameTreeNodeAsync(string treeName, int nodeId, string newNodeName);
        Task DeleteTreeNodeAsync(string treeName, int nodeId);
    }
}
