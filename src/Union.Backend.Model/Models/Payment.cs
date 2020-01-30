using System;
using System.Collections.Generic;
using System.Text;

namespace Union.Backend.Model.Models
{
    public class Payment
    {
        public Guid Id { get; set; }
        public int Sum { get; set; }
        public int State { get; set; }
        public Location Location { get; set; }
        public User Payer { get; set; }
        public User Collector { get; set; }
    }
}
