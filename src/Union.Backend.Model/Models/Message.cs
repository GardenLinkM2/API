﻿using System;
using System.Collections.Generic;

namespace Union.Backend.Model.Models
{
    public class Message : UniqueEntity, IPhotographable
    {
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public bool Read { get; set; }
        public Talk Talk{ get; set; }
        public User Sender { get; set; }
        public List<Photo<Message>> Photos { get; set; }
    }
}
