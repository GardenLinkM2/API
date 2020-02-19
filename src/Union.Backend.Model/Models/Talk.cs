using System.Collections.Generic;

namespace Union.Backend.Model.Models
{
    public class Talk : UniqueEntity
    {
        public string Subject { get; set; }
        public List<Message> Messages { get; set; }
        public bool Archive { get; set; }
        public User Sender { get; set; }
        public User Receiver { get; set; }
    }
}
