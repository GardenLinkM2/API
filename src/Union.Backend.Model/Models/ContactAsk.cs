using System;
using System.Collections.Generic;
using System.Text;

namespace Union.Backend.Model.Models
{
    public class ContactAsk
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public Guid Sender { get; set; }
        public Guid Receiver { get; set; }
    }
}
