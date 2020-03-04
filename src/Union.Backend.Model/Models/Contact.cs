using System;

namespace Union.Backend.Model.Models
{
    public class Contact : UniqueEntity
    {
        public Guid Me { get; set; }
        public User MyContact { get; set; }
        public Status Status { get; set; }
        public string FirstMessage { get; set; }
    }
}
