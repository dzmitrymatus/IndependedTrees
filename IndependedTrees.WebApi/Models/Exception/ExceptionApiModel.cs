namespace IndependedTrees.WebApi.Models.Exception
{
    public class ExceptionApiModel
    {
        public string? Type { get; set; }
        public Guid Id { get; set; }
        public ExceptionDataApiModel? Data { get; set; }
    }

    public class ExceptionDataApiModel
    {
        public string? Message { get; set; }
    }
}
