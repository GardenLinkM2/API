namespace Union.Backend.Model.Models
{
    public class Payment : UniqueEntity
    {
        public int Sum { get; set; }
        public int State { get; set; }
        public Leasing Leasing { get; set; }
        public User Payer { get; set; }
        public User Collector { get; set; }
    }
}
