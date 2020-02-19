using System;

namespace Union.Backend.Service.Dtos
{
    public class LeasingDto
    {
        public Guid Id { get; set; }
        public int Time { get; set; }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        public bool Renew { get; set; }
        public float Price { get; set; }
        public int State { get; set; }
        public GardenDto Garden { get; set; }
        public Guid Owner { get; set; }
        public Guid Renter { get; set; }
    }
}
