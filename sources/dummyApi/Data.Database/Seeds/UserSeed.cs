using Data.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Database.Seeds
{
    internal class UserSeed : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<UserEntity> builder)
        {
            var timeStamp = DateTime.Parse("2026-01-01T00:00:00");

            builder.HasData(
                new UserEntity
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "JohnDoe@gmail.com",
                    BlogId = 1,
                    CredentialsId = 1,
                    CreatedAt = timeStamp,
                    CreatedBy = "System",
                    UpdatedBy = null,
                    UpdatedAt = null
                }
            );
        }
    }
}
