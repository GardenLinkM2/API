namespace Union.Backend.Service.Results
{
    public class QueryResults<T>
    {
        public T Data { get; set; }
        public int? Count { get; set; }
    }
}
