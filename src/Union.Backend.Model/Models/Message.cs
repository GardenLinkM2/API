using System;
using System.Collections.Generic;

namespace Union.Backend.Model.Models
{
    public class Message : IPhotographable
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public bool Read { get; set; }
        public virtual Conversation Conversation { get; set; }
        public virtual User Sender { get; set; }
        public virtual List<Photo<Message>> Photos { get; set; }
    }
}
