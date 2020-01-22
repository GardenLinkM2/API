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
        public Garden Garden { get; set; }
        public User User { get; set; }
    }
}
