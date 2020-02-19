using System;

namespace Union.Backend.Model.Models
{
    public class Validation : UniqueEntity
    {
        public int State { get; set; }
        public Guid ForGarden { get; set; }
    }
}
