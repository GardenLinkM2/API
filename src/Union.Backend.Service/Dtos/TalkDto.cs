using System;

namespace Union.Backend.Service.Dtos
{
    public class TalkDto
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public bool Archive { get; set; }
        public UserDto Sender { get; set; }
        public UserDto Receiver { get; set; }
    }
}
