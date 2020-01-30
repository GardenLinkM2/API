using System;
using System.Collections.Generic;
using System.Text;

namespace Union.Backend.Model.Models
{
    public class Conversation //TODO: rajouter la list de Message
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public bool Archive { get; set; }
        public User Sender { get; set; }
        public User Receiver { get; set; }
    }
}
