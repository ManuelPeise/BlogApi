namespace Data.Database.Entities
{
    public class BlogEntity : AEntityBase
    {
        public string Name { get; set; } = string.Empty;
        public bool IsPrivate { get; set; }
        public ICollection<PostEntity> Posts { get; set; } = new List<PostEntity>();
        public ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
    }
}
