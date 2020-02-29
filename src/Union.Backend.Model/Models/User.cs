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
        public DateTime Inscription { get; set; } //TODO: changer pour un long (= Timestamp)
        public List<Contact> Contacts { get; set; }
        public Wallet Wallet { get; set; }
        public Photo<User> Photo { get; set; }
        public List<Garden> Gardens { get; set; }
    }
}
