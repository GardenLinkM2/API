using System;

namespace Union.Backend.Model.Models
{
    public class Report : UniqueEntity
    {
        public string Reason { get; set; }
        public string Comment { get; set; }
        public Garden OfGarden { get; set; }
        public Guid Reporter { get; set; }
    }
}
