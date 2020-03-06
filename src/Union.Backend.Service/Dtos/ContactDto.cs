using System;
using Union.Backend.Model.Models;

namespace Union.Backend.Service.Dtos
{
    public class ContactDto
    {
        public Guid Id { get; set; }
        public UserDto Contact { get; set; }
        public Status Status { get; set; }
        public string FirstMessage { get; set; }
    }
}
