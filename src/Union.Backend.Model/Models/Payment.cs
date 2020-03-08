using System;

namespace Union.Backend.Model.Models
{
    public class Payment : UniqueEntity
    {
        public int Sum { get; set; }
        public DateTime Date { get; set; }
        public Guid OfLeasing { get; set; }
        public Leasing Leasing { get; set; }
    }
}
