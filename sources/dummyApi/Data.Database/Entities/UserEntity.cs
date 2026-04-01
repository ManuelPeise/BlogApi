using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Database.Entities
{
    public class UserEntity : AEntityBase
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public byte[] ProfileImage { get; set; } = new byte[0];
        public DateTime? DateOfBirth { get; set; }
        public bool MarkedAsDeleted { get; set; } = false;
        public DateTime? MarkedAdDeletedAt { get; set; }
        public int CredentialsId { get; set; }
        [ForeignKey(nameof(CredentialsId))]
        public UserCredentialsEntity Credentials { get; set; }
        public int? AddressId { get; set; }
        [ForeignKey(nameof(AddressId))]
        public AddressEntity? Address { get; set; } = null!;
        public ICollection<BlogEntity> Blogs { get; set; } = new List<BlogEntity>();
    }
}
