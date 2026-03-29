namespace Data.Database.Entities
{
    public class UserCredentialsEntity: AEntityBase
    {
        public string PasswordHash { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
    }
}
