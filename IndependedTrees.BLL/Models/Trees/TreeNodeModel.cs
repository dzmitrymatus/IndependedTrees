namespace IndependedTrees.BLL.Models.Trees
{
    public class TreeNodeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public int? TreeId { get; set; }
        public IEnumerable<TreeNodeModel> Childrens { get; set; }
    }
}
