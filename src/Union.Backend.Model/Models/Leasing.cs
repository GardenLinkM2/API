using System;

namespace Union.Backend.Model.Models
{
    public class Leasing : UniqueEntity
    {
        public int Time { get; set; }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        public bool Renew { get; set; }
        public float Price { get; set; }
        public int State { get; set; }
        public Garden Garden { get; set; }
        public User Owner { get; set; }
        public User Renter { get; set; }
    }
}
