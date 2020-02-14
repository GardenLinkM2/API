using System;

namespace Union.Backend.Service.Dtos
{
    public class DemandDto
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public UserDto Sender { get; set; }
        public UserDto Receiver { get; set; }
    }
}
