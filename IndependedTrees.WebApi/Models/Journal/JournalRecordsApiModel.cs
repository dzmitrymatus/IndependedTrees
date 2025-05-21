namespace IndependedTrees.WebApi.Models.Journal
{
    public class JournalRecordsApiModel
    {
        public int Skip { get; set; }
        public int Count { get; set; }
        public IEnumerable<JournalRecordApiModel>? Items { get; set; }
    }
}
