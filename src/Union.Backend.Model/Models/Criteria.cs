using System;

namespace Union.Backend.Model.Models
{
    public enum Orientation
    {
        Unset = 0,
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
        private TimeSpan? RealLocationTime { get; set; }
        public long? LocationTime 
        { 
            get => RealLocationTime?.ToSeconds(); 
            set => RealLocationTime = value?.ToTimeSpan(); 
        }
        public int? Area { get; set; }
        public float? Price { get; set; }
        public Orientation? Orientation { get; set; }
        public string TypeOfClay { get; set; }
        public bool? Equipments { get; set; }
        public bool? WaterAccess { get; set; }
        public bool? DirectAccess { get; set; }
    }
}
