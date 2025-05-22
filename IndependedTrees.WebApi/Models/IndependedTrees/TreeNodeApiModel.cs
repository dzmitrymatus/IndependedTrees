namespace IndependedTrees.WebApi.Models.IndependedTrees
{
    public class TreeNodeApiModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<TreeNodeApiModel> Childrens { get; set; }
    }
}
