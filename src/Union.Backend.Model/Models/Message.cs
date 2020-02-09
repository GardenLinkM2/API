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
        public Guid Conversation { get; set; }
        public Guid Sender { get; set; }
        public List<Photo<Message>> Photos { get; set; }
    }
}
