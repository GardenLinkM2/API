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
        public virtual Location Location { get; set; }
        public virtual User Payer { get; set; }
        public virtual User Collector { get; set; }
    }
}
