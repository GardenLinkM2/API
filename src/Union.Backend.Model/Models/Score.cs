using System;

namespace Union.Backend.Model.Models
{
    public class Score : UniqueEntity
    {
        public int Mark { get; set; }
        public string Comment { get; set; }
        public Guid Rater { get; set; }
        public Guid Rated { get; set; }
        public Boolean Reported { get; set; }
    }
}
