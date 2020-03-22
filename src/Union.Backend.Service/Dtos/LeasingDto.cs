using System;
using Union.Backend.Model.Models;

namespace Union.Backend.Service.Dtos
{
    public class LeasingDto
    {
        public Guid Id { get; set; }
        public double Creation { get; set; }
        public long? Time { get; set; }
        public double? Begin { get; set; }
        public double? End { get; set; }
        public bool? Renew { get; set; }
        public LeasingStatus? State { get; set; }
        public Guid Garden { get; set; }
        public Guid Renter { get; set; }
        public Guid Owner { get; set; }
    }
}
