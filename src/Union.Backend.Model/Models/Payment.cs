using System;

namespace Union.Backend.Model.Models
{
    public class Payment : UniqueEntity
    {
        public int Sum { get; set; }
        public int State { get; set; }
        public Leasing Leasing { get; set; }
    }
}
