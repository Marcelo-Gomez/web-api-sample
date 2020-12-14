﻿using Microsoft.IdentityModel.Tokens;
using System;

namespace web_api_sample.api.Authentication
{
    public class JwtIssuerOptions
    {
        public string Secret { get; set; }

        public string Issuer { get; set; }

        public string Subject { get; set; }

        public string Audience { get; set; }

        public DateTime Expiration => IssuedAt.Add(ValidFor);

        public DateTime NotBefore => DateTime.UtcNow;

        public DateTime IssuedAt => DateTime.UtcNow;

        public TimeSpan ValidFor { get; set; } = TimeSpan.FromMinutes(120);

        public string JtiGenerator => Guid.NewGuid().ToString();

        public SigningCredentials SigningCredentials { get; set; }
    }
}