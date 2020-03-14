using System.ComponentModel.DataAnnotations.Schema;

namespace Union.Backend.Model.Models
{
    public class Location : UniqueEntity
    {
        public int StreetNumber { get; set; }
        public string Street { get; set; }
        public int PostalCode { get; set; }
        public string City { get; set; }

        private double Longitude { get; set; }
        private double Latitude { get; set; }
        [NotMapped]
        public (double longitude, double latitude) Coordinates 
        { 
            get => (Longitude, Latitude);
            set { Longitude = value.longitude; Latitude = value.latitude; }
        }
    }
}
