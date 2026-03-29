using Data.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Database.Seeds
{
    internal class BlogSeed : IEntityTypeConfiguration<BlogEntity>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<BlogEntity> builder)
        {
            var timeStamp = DateTime.Parse("2026-01-01T00:00:00");

            builder.HasData(
                new BlogEntity
                {
                    Id = 1,
                    Name = "Tech Blog",
                    IsPrivate = true,
                    CreatedAt = timeStamp,
                    CreatedBy = "System",
                    UpdatedBy = null,
                    UpdatedAt = null
                }
            );
        }
    }
}
