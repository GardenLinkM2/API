using System;

namespace Union.Backend.Service.Dtos
{
    public class ContactDto
    {
        public Guid Id { get; set; }
        public  bool Accept { get; set; }
        public UserDto UserOne { get; set; }
        public UserDto UserTwo { get; set; }
    }
}
