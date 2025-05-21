namespace IndependedTrees.DAL.Models.Journal
{
    public class JournalRecord
    {
        public Guid EventId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string? QueryParameters { get; set; }
        public required string StackTrace { get; set; }
    }
}
