namespace Data.Database.Entities
{
    public class UserEntity : AEntityBase
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int? BlogId { get; set; }
        public BlogEntity? Blog { get; set; }
        public int CredentialsId { get; set; }
        public UserCredentialsEntity Credentials { get; set; }
    }
}
