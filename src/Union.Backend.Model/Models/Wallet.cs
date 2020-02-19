using System;

namespace Union.Backend.Model.Models
{
    public class Wallet : UniqueEntity
    {
        public int Balance { get; set; }
        public Guid OfUser { get; set; }
    }
}
