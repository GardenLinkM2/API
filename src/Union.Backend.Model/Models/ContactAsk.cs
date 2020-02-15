namespace Union.Backend.Model.Models
{
    public class ContactAsk : UniqueEntity
    {
        public string Message { get; set; }
        public User Sender { get; set; }
        public User Receiver { get; set; }
    }
}
