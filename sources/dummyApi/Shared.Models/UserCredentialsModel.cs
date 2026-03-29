namespace Shared.Models
{
    public class UserCredentialsModel
    {
        public int Id { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
    }
}
