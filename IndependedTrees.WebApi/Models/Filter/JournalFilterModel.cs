namespace IndependedTrees.WebApi.Models.Filter
{
    public class JournalFilterModel
    {
        public DateTime? From {  get; set; }
        public DateTime? To { get; set; }
        public string? Search {  get; set; }
    }
}
