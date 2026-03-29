namespace Shared.Models
{
    public class JwtTokenData
    {
        public string SecurityKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpiresInSeconds { get; set; }
    }
}
