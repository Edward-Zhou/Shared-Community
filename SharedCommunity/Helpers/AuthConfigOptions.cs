using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedCommunity.Helpers
{
    public class AuthConfigOptions
    {
        public JwtBearerOptions JwtBearerConfig { get; set; }
    }
    public class JwtBearerOptions
    {
        public bool IsEnabled { get; set; }
        public string Path { get; set; }
        public string SecurityKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Expiration { get; set; }
    }
}
