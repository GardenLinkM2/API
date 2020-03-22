using System;
using System.ComponentModel.DataAnnotations.Schema;

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
        public DateTime Creation { get; set; }
        [NotMapped]
        public TimeSpan Time { get => End.Subtract(Begin); }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        public bool Renew { get; set; }
        public LeasingStatus State { get; set; }
        public Garden Garden { get; set; }
        public User Renter { get; set; }
        public Payment Payment { get; set; }
        [NotMapped]
        public int MonthDifference { get => End.MonthDifference(Begin); }
    }
}
