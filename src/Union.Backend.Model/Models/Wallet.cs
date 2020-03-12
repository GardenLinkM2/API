using System;

namespace Union.Backend.Model.Models
{
    public class Wallet : UniqueEntity
    {
        public float Balance { get; set; }
        public Guid OfUser { get; set; }
    }
}
