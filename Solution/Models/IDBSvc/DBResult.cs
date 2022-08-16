namespace Models.IDBSvc
{
    public class DBResult<T>
    {
        public int ErrorCode { get; set; }
        public T Content { get; set; }
        public Exception Exception { get; set; }
    }
}