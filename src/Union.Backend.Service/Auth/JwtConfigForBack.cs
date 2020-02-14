using Jose;
using System;

namespace Union.Backend.Service.Auth
{
    public static class JwtConfigForBack
    {
        public static readonly string Kid = "back";
        public static readonly JwsAlgorithm JwsAlgorithm = JwsAlgorithm.HS512;
        public static readonly int BaseAddDays = 10;
    }
}
