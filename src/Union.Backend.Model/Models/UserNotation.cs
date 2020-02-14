using System;
using System.Collections.Generic;
using System.Text;

namespace Union.Backend.Model.Models
{
    public class UserNotation
    {
        public Guid Id { get; set; }
        public int Note { get; set; }
        public string Comment { get; set; }
        public User Rater { get; set; }
        public User Rated { get; set; }
    }
}
