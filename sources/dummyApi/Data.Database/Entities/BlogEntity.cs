using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Database.Entities
{
    public class BlogEntity : AEntityBase
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public byte[]? Image { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsMarkedAsDeleted { get; set; } = false;
        public DateTime? MarkedAsDeletedAt { get; set; } = null;
        public ICollection<PostEntity> Posts { get; set; } = new List<PostEntity>();
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; } = null!;
    }
}
