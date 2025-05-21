namespace IndependedTrees.WebApi.Models.Journal
{
    public class JournalRecordApiModel
    {
        public Guid EventId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string? QueryParameters { get; set; }
        public string? StackTrace { get; set; }
    }
}
