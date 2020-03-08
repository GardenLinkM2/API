using System;

namespace Union.Backend.Model.Models
{
    public enum Orientation
    {
        North = 1,
        East = 2,
        South = 4,
        West = 8,
        NorthEast = North | East,
        NorthWest = North | West,
        SouthEast = South | East,
        SouthWest = South | West,
    }

    public class Criteria : UniqueEntity
    {
        public TimeSpan LocationTime { get; set; }
        public int Area { get; set; }
        public double Price { get; set; }
        public Orientation Orientation { get; set; }
        public string TypeOfClay { get; set; }
        public bool Equipments { get; set; }
        public bool WaterAccess { get; set; }
        public bool DirectAccess { get; set; }
    }
}
