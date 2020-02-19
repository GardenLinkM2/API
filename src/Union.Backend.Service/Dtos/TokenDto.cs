using System;

namespace Union.Backend.Service.Dtos
{
    public class TokenDto
    {
        public string Token { get; set; }
        [SwaggerExclude]
        public bool? IsAdmin { get; set; }
        [SwaggerExclude]
        public string Uuid { get; set; }
        [SwaggerExclude]
        public string Emitter { get; set; }
        [SwaggerExclude]
        public string TokenId { get; set; }
        [SwaggerExclude]
        public string Username { get; set; }
        [SwaggerExclude]
        public DateTime? ExpirationTime { get; set; }
    }
}
