using System;

namespace Domain
{
    public class RefreshToken
    {
        public RefreshToken() { }

        public RefreshToken(string refreshToken)
        {
            Token = refreshToken;
        }

        public int Id { get; set; }
        public AppUser AppUser { get; set; }
        public string Token { get; }
        public DateTime Expires { get; set; } = DateTime.UtcNow.AddDays(7);

        public bool IsExpired => DateTime.UtcNow >= Expires;

        public DateTime? Revoked { get; set; }

        public bool IsActive => Revoked == null && !IsExpired;
    }
}
