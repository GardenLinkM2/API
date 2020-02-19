using System;

namespace Union.Backend.Model.Models
{
    public enum ContactStatus
    {
        Pending,
        Accepted,
        Refused
    }

    public class Contact : UniqueEntity
    {
        public Guid Me { get; set; }
        public User MyContact { get; set; }
        public ContactStatus Status { get; set; }
        public string FirstMessage { get; set; }
    }
}
