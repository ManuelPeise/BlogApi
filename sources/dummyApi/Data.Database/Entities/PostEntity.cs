using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Database.Entities
{
    public class PostEntity : AEntityBase
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public byte[]? Image { get; set; } = null;
        public int BlogId { get; set; }
        public BlogEntity Blog { get; set; }
    }
}
