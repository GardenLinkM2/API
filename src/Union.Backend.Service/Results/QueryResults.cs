using System.Collections.Generic;

namespace Union.Backend.Service.Results
{
    public class QueryResults<T>
    {
        public List<T> Data { get; set; }
        public int Count { get => Data.Count; }
    }
}
