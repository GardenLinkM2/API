using System;

namespace Union.Backend.Service.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public PhotoDto Photo { get; set; }
        [SwaggerExclude]
        public WalletDto Wallet { get; set; }
    }
}
