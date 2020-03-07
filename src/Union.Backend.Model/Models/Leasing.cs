using System;

namespace Union.Backend.Model.Models
{
    public enum LeasingStatus
    {
        InDemand = 0,
        Refused,
        InProgress,
        Finished
    }

    public class Leasing : UniqueEntity
    {
        public TimeSpan Time { get; set; }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        public bool Renew { get; set; }
        public LeasingStatus State { get; set; }
        public Guid Garden { get; set; }
        public Guid Renter { get; set; }
        public Guid Owner { get; set; }
    }
}
