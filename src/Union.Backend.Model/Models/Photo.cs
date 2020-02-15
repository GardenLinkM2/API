using System;

namespace Union.Backend.Model.Models
{
    public interface IPhotographable { }

    public class Photo<TRelated> where TRelated : IPhotographable
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public Guid RelatedTo { get; set; }
    }
}
