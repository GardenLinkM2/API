﻿using System;
using System.Collections.Generic;

namespace Union.Backend.Service.Dtos
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public bool Read { get; set; }
        public ConversationDto Conversation { get; set; }
        public UserDto Sender { get; set; }
        public List<PhotoDto> Photos { get; set; }
    }
}
