using System;
using System.Collections.Generic;

namespace Union.Backend.Model.Models
{
    public class Garden : IPhotographable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public bool Reserve { get; set; }
        public string Type { get; set; }
        public int MinUse { get; set; }
        public Guid Owner { get; set; }
        public Guid Tenant { get; set; }
        public Guid Validation { get; set; }
        public Guid Criteria { get; set; }
        public List<Photo<Garden>> Photos { get; set; }
    }
}
