using System;
using System.Collections.Generic;
using System.Text;

namespace Union.Backend.Service.Dtos
{
    public class CriteriaDto
    {
        public Guid Id { get; set; }
        public TimeSpan LocationTime { get; set; }
        public int Area { get; set; }
        public double Price { get; set; }
        public LocationDto Location { get; set; }
        public string Orientation { get; set; }
        public string TypeOfClay { get; set; }
        public bool Equipments { get; set; }
        public bool WaterAccess { get; set; }
        public bool DirectAccess { get; set; }
    }
}
