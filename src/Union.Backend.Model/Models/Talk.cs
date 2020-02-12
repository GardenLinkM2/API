using System;
using System.Collections.Generic;
using System.Text;

namespace Union.Backend.Model.Models
{
    public class Talk //TODO: rajouter la list de Message
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public bool Archive { get; set; }
        public Guid Sender { get; set; }
        public Guid Receiver { get; set; }
    }
}
