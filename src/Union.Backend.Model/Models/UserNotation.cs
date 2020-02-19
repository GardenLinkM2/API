namespace Union.Backend.Model.Models
{
    public class UserNotation : UniqueEntity
    {
        public int Note { get; set; }
        public string Comment { get; set; }
        public User Rater { get; set; }
        public User Rated { get; set; }
    }
}
