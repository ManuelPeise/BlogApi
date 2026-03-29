namespace Shared.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? CreatedBy { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public string? UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public int? BlogId { get; set; }
        public BlogModel? Blog { get; set; } = new BlogModel();
        public UserCredentialsModel Credentials { get; set; } = new UserCredentialsModel();
    }
}
