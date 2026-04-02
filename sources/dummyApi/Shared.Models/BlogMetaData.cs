namespace Shared.Models
{
    public class BlogMetaData
    {
        public int BlogId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public byte[]? Image { get; set; }
        public int PostCount { get; set; }
        public DateTime? LastPostingDate { get; set; }
    }
}
