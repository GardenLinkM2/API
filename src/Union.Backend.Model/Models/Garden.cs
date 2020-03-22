using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Union.Backend.Model.Models
{
    public class Garden : UniqueEntity, IPhotographable
    {
        public string Name { get; set; }
        [NotMapped]
        public bool IsReserved { get => 
            Leasings?.Any(l => l.State.Equals(LeasingStatus.InProgress)) ?? false; 
        }
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
