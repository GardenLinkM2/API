using System;

namespace Union.Backend.Service.Dtos
{
    public class TokenDto
    {
        public string Token { get; set; }
        [SwaggerExclude]
        public string Sub { get; set; }
        [SwaggerExclude]
        public bool? IsAdmin { get; set; }
        [SwaggerExclude]
        public string Uuid { get; set; }
        [SwaggerExclude]
        public string Emitter { get; set; }
        [SwaggerExclude]
        public Guid? Jti { get; set; }
        [SwaggerExclude]
        public long? Exp { get; set; }
    }
}
