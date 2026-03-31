namespace Shared.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public byte[] ProfileImage { get; set; } = new byte[0];
        public string? CreatedBy { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public string? UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public int? BlogId { get; set; }
        public BlogModel? Blog { get; set; }
        public UserCredentialsModel? Credentials { get; set; }
        public int AddressId { get; set; }
        public AddressModel? Address { get; set; }
    }
}
