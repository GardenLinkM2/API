using System;
using System.Collections.Generic;

namespace Union.Backend.Model.Models
{
    public class Garden : UniqueEntity, IPhotographable
    {
        public string Name { get; set; }
        public bool IsReserved { get; set; }
        public int MinUse { get; set; }
        public string Description { get; set; }
        public Location Location { get; set; }
        public Status Validation { get; set; }
        public Criteria Criteria { get; set; }
        public List<Photo<Garden>> Photos { get; set; }
        public User Owner { get; set; }
        public ICollection<Leasing> Leasings { get; set; }
        public ICollection<Report> Reports { get; set; }
    }
}
