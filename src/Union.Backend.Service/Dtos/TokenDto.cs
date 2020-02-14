using System;

namespace Union.Backend.Service.Dtos
{
    public class TokenDto
    {
        public string Token { get; set; }
        public bool? IsAdmin { get; set; }
        public string Uuid { get; set; }
        public string Emitter { get; set; }
        public string TokenId { get; set; }
        public string Username { get; set; }
        public DateTime? ExpirationTime { get; set; }
    }
}
