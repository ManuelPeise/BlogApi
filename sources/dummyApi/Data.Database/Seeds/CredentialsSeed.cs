using Data.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Database.Seeds
{
    internal class CredentialsSeed : IEntityTypeConfiguration<UserCredentialsEntity>
    {
        public void Configure(EntityTypeBuilder<UserCredentialsEntity> builder)
        {
            var timeStamp = DateTime.Parse("2026-01-01T00:00:00");

            builder.HasData(
               new UserCredentialsEntity
               {
                   Id = 1,
                   PasswordHash = "$2a$12$dxAIJV8PCdWgJIh46rkTy.LDXibzckw/e5qJmCCHPnBfpw4UVGdQW",
                   RefreshToken = null,
                   CreatedAt = timeStamp,
                   CreatedBy = "System",
                   UpdatedBy = null,
                   UpdatedAt = null
               });
        }
    }
}
