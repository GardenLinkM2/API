using System;

namespace Union.Backend.Service.Dtos
{
    public class TalkDto
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public bool Archive { get; set; }
        public Guid Sender { get; set; }
        public Guid Receiver { get; set; }
    }
}
