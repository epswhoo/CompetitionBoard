namespace CompetitionBoard_Net8.Models.Results
{
    public class Result<T>
    {
        public int ErrorCode { get; set; }
        public T Content { get; set; }
        public Exception Exception { get; set; }
    }
}