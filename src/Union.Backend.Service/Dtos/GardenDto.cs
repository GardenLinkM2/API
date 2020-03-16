using System;
using System.Collections.Generic;
using Union.Backend.Model.Models;

namespace Union.Backend.Service.Dtos
{
    public class GardenDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool? IsReserved { get; set; }
        public int? MinUse { get; set; }
        public string Description { get; set; }
        public LocationDto Location { get; set; }
        public Guid Owner { get; set; }
        public Status Validation { get; set; }
        public CriteriaDto Criteria { get; set; }
        public ICollection<PhotoDto> Photos { get; set; }
        public ICollection<ReportDto> Reports { get; set; }
    }
}
