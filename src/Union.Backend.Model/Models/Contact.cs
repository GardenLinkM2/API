using System;
using System.Collections.Generic;
using System.Text;

namespace Union.Backend.Model.Models
{
    public class Contact
    {
        public Guid Id { get; set; }
        public bool Accept { get; set; }
        public User UserOne { get; set; }
        public User UserTwo { get; set; }
        public ContactAsk Ask { get; set; }
    }
}
