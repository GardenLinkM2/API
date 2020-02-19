using System;

namespace Union.Backend.Model.Models
{
    public class GardenNotation : UniqueEntity
    {
        public int Note { get; set; }
        public string Comment { get; set; }
        public Guid Garden { get; set; }
        public Guid User { get; set; }
    }
}
