using Union.Backend.Model.Models;

namespace Union.Backend.Service.Dtos
{
    public class CriteriaDto
    {
        public long? LocationTime { get; set; }
        public int? Area { get; set; }
        public double? Price { get; set; }
        public Orientation? Orientation { get; set; }
        public string TypeOfClay { get; set; }
        public bool? Equipments { get; set; }
        public bool? WaterAccess { get; set; }
        public bool? DirectAccess { get; set; }
    }
}
