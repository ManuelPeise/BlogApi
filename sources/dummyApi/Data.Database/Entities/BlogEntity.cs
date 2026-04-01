namespace Data.Database.Entities
{
    public class BlogEntity : AEntityBase
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public byte[]? Image { get; set; }
        public bool IsPrivate { get; set; }
        public ICollection<PostEntity> Posts { get; set; } = new List<PostEntity>();
        public ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
    }
}
