using System;

namespace Union.Backend.Service.Dtos
{
    public class ScoreDto
    {
        public Guid Id { get; set; }
        public int Note { get; set; }
        public string Comment { get; set; }
        public Guid Rater { get; set; }
        public Guid Rated { get; set; } //Garden ou User
    }
}
