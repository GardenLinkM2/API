using System;
using System.Collections.Generic;
using System.Text;

namespace Union.Backend.Model.Models
{
    public class Location
    {
        public Guid Id { get; set; }
        public int Time { get; set; }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        public bool Renew { get; set; }
        public float Price { get; set; }
        public int State { get; set; }
        public virtual Garden Garden { get; set; }
        public virtual User Owner { get; set; }
        public virtual User Renter { get; set; }
    }
}
