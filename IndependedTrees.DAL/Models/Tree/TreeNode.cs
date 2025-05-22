namespace IndependedTrees.DAL.Models.Tree
{
    public class TreeNode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public int? TreeId { get; set; }
    }
}
