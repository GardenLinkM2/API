using System;
using System.Collections.Generic;

namespace Union.Backend.Model.Models
{
    public class Message : UniqueEntity, IPhotographable
    {
        public string Text { get; set; }
        public DateTime CreationDate { get; set; } //TODO: transformer en long
        public bool IsReaded { get; set; }
        public Guid Sender { get; set; }
        public Guid OfTalk { get; set; }
        public List<Photo<Message>> Photos { get; set; }
    }
}
