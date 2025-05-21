namespace IndependedTrees.BLL.Models.Journal
{
    public class JournalRecordModel
    {
        public Guid EventId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string? QueryParameters { get; set; }
        public required string StackTrace { get; set; }
    }
}
