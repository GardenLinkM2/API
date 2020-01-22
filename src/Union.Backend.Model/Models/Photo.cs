using System;
using System.Collections.Generic;
using System.Text;

namespace Union.Backend.Model.Models
{
    public class Photo
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
    }
}
