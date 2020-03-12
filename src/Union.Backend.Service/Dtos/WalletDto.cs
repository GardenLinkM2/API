﻿using System;

namespace Union.Backend.Service.Dtos
{
    public class WalletDto
    {
        [SwaggerExclude]
        public Guid Id { get; set; }
        public float Balance { get; set; }
    }
}
