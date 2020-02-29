using System;
using System.Collections.Generic;

namespace Union.Backend.Service.Dtos
{
    public class TalkDto
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public bool IsArchived { get; set; }
        public Guid Sender { get; set; }
        public Guid Receiver { get; set; }
        public List<MessageDto> Messages { get; set; }
    }
}
