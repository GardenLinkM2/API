namespace Union.Backend.Service.Dtos
{
    /// <summary>
    /// Dialog class with Authentication Server
    /// </summary>
    public class AuthUserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Phone { get; set; }
        public bool IsAdmin { get; set; }
    }
}
