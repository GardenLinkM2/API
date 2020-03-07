using System;
using Union.Backend.Model.Models;

namespace Union.Backend.Service.Dtos
{
    public class LeasingDto
    {
        public Guid Id { get; set; }
        public long? Time { get; set; }
        public DateTime? Begin { get; set; }
        public DateTime? End { get; set; }
        public bool? Renew { get; set; }
        public LeasingStatus? State { get; set; }
        public Guid Garden { get; set; }
        [SwaggerExclude]
        public Guid Renter { get; set; }
        [SwaggerExclude]
        public Guid Owner { get; set; }
    }
}
