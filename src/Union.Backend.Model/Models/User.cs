using System;
using System.Collections.Generic;

namespace Union.Backend.Model.Models
{
    public class User : UniqueEntity, IPhotographable
    {
        public string Mail { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Inscription { get; set; }
        public ICollection<Contact> Contacts { get; set; }
        public Wallet Wallet { get; set; }
        public Photo<User> Photo { get; set; }
        public ICollection<Garden> Gardens { get; set; }
        public ICollection<Leasing> AsRenter { get; set; }
        public ICollection<Score> AsRater { get; set; }
    }
}
