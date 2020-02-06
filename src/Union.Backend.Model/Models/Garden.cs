using System;
using System.Collections.Generic;
using System.Text;

namespace Union.Backend.Model.Models
{
    public class Garden : IPhotographable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        //TODO: ajouter Localisation avec une nouvelle classe dédiée
        public int Size { get; set; }
        public bool Reserve { get; set; }
        public string Type { get; set; }
        public int MinUse { get; set; }
        public virtual User Owner { get; set; }
        public virtual User Tenant { get; set; }
        public virtual Validation Validation { get; set; }
        public virtual Using Use { get; set; }
        public virtual Characteristic Carac { get; set; }
        public virtual List<Photo<Garden>> Photos { get; set; }
    }
}
