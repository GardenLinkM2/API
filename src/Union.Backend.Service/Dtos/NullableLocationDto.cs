namespace Union.Backend.Service.Dtos
{
    public class NullableLocationDto
    {
        public int? StreetNumber { get; set; }
        public string Street { get; set; }
        public int? PostalCode { get; set; }
        public string City { get; set; }
    }
}
