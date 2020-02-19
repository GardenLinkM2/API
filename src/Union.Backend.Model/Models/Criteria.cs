using System;

namespace Union.Backend.Model.Models
{
    public class Criteria : UniqueEntity
    //TODO: Une Table (id, nom, type (visuel) <-probablement la même chose-> type (valeur), valuePossible)
    //valuePossible (table générique ?) T ou T[]
    //-> probablement une Table de jointure avec Jardin
    {
        public TimeSpan LocationTime { get; set; } //TODO: utiliser des long (= Timestamp)
        public int Area { get; set; }
        public double Price { get; set; }
        public Location Location { get; set; }
        public string Orientation { get; set; }
        public string TypeOfClay { get; set; }
        public bool Equipments { get; set; }
        public bool WaterAccess { get; set; }
        public bool DirectAccess { get; set; }
        public Guid ForGarden { get; set; }
    }
}
