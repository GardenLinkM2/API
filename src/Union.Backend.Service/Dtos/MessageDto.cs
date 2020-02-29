using System;
using System.Collections.Generic;

namespace Union.Backend.Service.Dtos
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsReaded { get; set; }
        public Guid Sender { get; set; }
        public List<PhotoDto> Photos { get; set; }
    }
}
