using System;
using System.Collections.Generic;
using Union.Backend.Model.Models;

namespace Union.Backend.Service.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string Mail { get; set; }
        public string PhoneNumber { get; set; }
        public List<PhotoDto> Photos { get; set; }
        public WalletDto Wallet { get; set; }
    }
}
