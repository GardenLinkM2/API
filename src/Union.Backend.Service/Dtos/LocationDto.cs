namespace Union.Backend.Service.Dtos
{
    public struct Coordinates
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }

    public class LocationDto
    {
        public int StreetNumber { get; set; }
        public string Street { get; set; }
        public int PostalCode { get; set; }
        public string City { get; set; }

        public Coordinates LongitudeAndLatitude { get; set; }
    }
}
