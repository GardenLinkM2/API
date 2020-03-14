using System;

namespace Union.Backend.Model.Models
{
    public class Location : UniqueEntity
    {
        public int StreetNumber { get; set; }
        public string Street { get; set; }
        public int PostalCode { get; set; }
        public string City { get; set; }

        public Tuple<double, double> LongitudeAndLatitude { get; set; }
    }
}
