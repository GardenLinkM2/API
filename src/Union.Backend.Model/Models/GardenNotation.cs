using System;
using System.Collections.Generic;
using System.Text;

namespace Union.Backend.Model.Models
{
    public class GardenNotation
    {
        public Guid Id { get; set; }
        public int Note { get; set; }
        public string Comment { get; set; }
        public virtual Garden Garden { get; set; }
        public virtual User User { get; set; }
    }
}
