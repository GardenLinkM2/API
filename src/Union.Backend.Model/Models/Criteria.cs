using System;
using System.Collections.Generic;
using System.Text;

namespace Union.Backend.Model.Models
{
    public class Criteria 
        //TODO: Une Table (id, nom, type (visuel) <-probablement la même chose-> type (valeur), valuePossible)
        //valuePossible (table générique ?) T ou T[]
        //-> probablement une Table de jointure avec Jardin
    {
        public Guid Id { get; set; }
        public TimeSpan LocationTime { get; set; }
        public int Area { get; set; }
        public double Price { get; set; }
        public Guid Location { get; set; }
        public string Orientation { get; set; }
        public string TypeOfClay { get; set; }
        public bool Equipments { get; set; }
        public bool WaterAccess { get; set; }
        public bool DirectAccess { get; set; }
    }
}
