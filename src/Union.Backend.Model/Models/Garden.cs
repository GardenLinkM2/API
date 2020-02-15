using System;
using System.Collections.Generic;

namespace Union.Backend.Model.Models
{
    public class Garden : UniqueEntity, IPhotographable
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public bool Reserve { get; set; }
        public string Type { get; set; }
        public int MinUse { get; set; }
        public User Owner { get; set; }
        public User Tenant { get; set; }
        public Validation Validation { get; set; }
        public Criteria Criteria { get; set; }
        public List<Photo<Garden>> Photos { get; set; }
        public Guid IdOwner { get; set; }
        public Guid IdTenant { get; set; }
        public Guid IdValidation { get; set; }
        public Guid IdCriteria { get; set; }
    }
}
