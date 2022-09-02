namespace Models.Results
{
    public class MultiResult<T>
    {
        public IEnumerable<Result<T>> Results { get; set; }
        public IEnumerable<T> Content { get; set; }
    }
}