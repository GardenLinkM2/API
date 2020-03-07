using System;
using System.Collections.Generic;

namespace Union.Backend.Model.Models
{
    public class Garden : UniqueEntity, IPhotographable
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public bool IsReserved { get; set; }
        public int MinUse { get; set; }
        public string Description { get; set; }
        public Location Location { get; set; }
        public User Tenant { get; set; }
        public Status Validation { get; set; }
        public Criteria Criteria { get; set; }
        public List<Photo<Garden>> Photos { get; set; }
        public Guid Owner { get; set; }
    }
}
