using System;

namespace Union.Backend.Service.Dtos
{
    public class LocationDto
    {
        public int StreetNumber { get; set; }
        public string Street { get; set; }
        public int PostalCode { get; set; }
        public string City { get; set; }

        public Tuple<double, double> LongitudeAndLatitude { get; set; }
    }
}
