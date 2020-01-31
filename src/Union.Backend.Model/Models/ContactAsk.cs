using System;
using System.Collections.Generic;
using System.Text;

namespace Union.Backend.Model.Models
{
    public class ContactAsk
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public User Sender { get; set; }
        public User Receiver { get; set; }
    }
}
