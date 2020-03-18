using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Union.Backend.Model.Models
{
    public class Wallet : UniqueEntity
    {
        public float TrueBalance { get; set; }
        [NotMapped]
        public float RealTimeBalance 
        {
            get => TrueBalance - (OfUser.AsRenter?.Where(l => l.State.Equals(LeasingStatus.InDemand))
                                                  .Sum(l => l.Garden.Criteria.Price ?? 0) 
                                 ?? 0);
        }
        public Guid OfUserId { get; set; }
        public User OfUser { get; set; }
    }
}
