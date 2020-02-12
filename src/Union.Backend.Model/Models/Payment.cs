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
        public Guid Leasing { get; set; }
        public Guid Payer { get; set; }
        public Guid Collector { get; set; }
    }
}
