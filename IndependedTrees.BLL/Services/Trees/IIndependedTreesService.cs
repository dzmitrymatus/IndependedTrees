using IndependedTrees.BLL.Models.Trees;

namespace IndependedTrees.BLL.Services.Trees
{
    public interface IIndependedTreesService
    {
        Task<TreeNodeModel> GetTreeAsync(string treeName);
        Task CreateTreeNodeAsync(string treeName, int parentNodeId, string nodeName);
    }
}
