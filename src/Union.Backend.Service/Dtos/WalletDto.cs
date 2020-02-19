using System;

namespace Union.Backend.Service.Dtos
{
    public class WalletDto
    {
        [SwaggerExclude]
        public Guid Id { get; set; }
        public int Balance { get; set; }
    }
}
