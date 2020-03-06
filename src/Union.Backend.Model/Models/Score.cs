using System;

namespace Union.Backend.Model.Models
{
    public class Score : UniqueEntity
    {
        public int Mark { get; set; }
        public string Comment { get; set; }
        public User Rater { get; set; }
        public Guid Rated { get; set; }
    }
}
