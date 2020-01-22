using System;
using System.Collections.Generic;
using System.Text;

namespace Union.Backend.Model.Models
{
    public class Garden
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public bool Reserve { get; set; }
        public string Type { get; set; }
        public int MinUse { get; set; }
        public User Owner { get; set; }
        public User Tenant { get; set; }
        public Validation Validation { get; set; }
        public Using Use { get; set; }
        public Characteristic Carac { get; set; }
        public List<Photo> Photos { get; set; }
    }
}
