﻿using System;
using System.Collections.Generic;

namespace Union.Backend.Model.Models
{
    public class User : IPhotographable
    {
        public Guid Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Mail { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public DateTime Inscription { get; set; }
        public bool Admin { get; set; }
        public Wallet Wallet { get; set; }
        public List<Photo<User>> Photos { get; set; }
    }
}
