using System;
using System.Collections.Generic;
using Union.Backend.Model.Models;

namespace Union.Backend.Service.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        [SwaggerExclude]
        public List<PhotoDto> Photos { get; set; }
        [SwaggerExclude]
        public Wallet Wallet { get; set; }
    }
}
