using System;

namespace Union.Backend.Model.Models
{
    public class Validation : UniqueEntity
    {
        public Guid ForGarden { get; set; }
        public Status State { get; set; }
    }
}
