using System;
using System.Collections.Generic;
using System.Text;

namespace Union.Backend.Model.Models
{
    public class Contact //TODO: vérifier si c'est bon (je suis pas sur des liens et de l'utilité de ContactAsk)
    {
        public Guid Id { get; set; }
        public bool Accept { get; set; }
        public Guid UserOne { get; set; }
        public Guid UserTwo { get; set; }
        public Guid Ask { get; set; }
    }
}
