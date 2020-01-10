using System;
using Microsoft.IdentityModel.Tokens;

namespace Template.Core.Settings
{
    // https://tools.ietf.org/html/rfc7519
    public class TokenSettings
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int LifetimeInMinutes { get; set; }

        public SigningCredentials SigningCredentials { get; set; }

        public DateTime Expiration => this.IssuedAt.Add(this.ValidFor);

        public DateTime NotBefore => this.IssuedAt;

        public DateTime IssuedAt => DateTime.UtcNow;

        public string JtiGenerator => Guid.NewGuid().ToString();

        public TimeSpan ValidFor => TimeSpan.FromMinutes(this.LifetimeInMinutes);
    }
}
