using Data.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Database.Seeds
{
    internal class CountrySeed : IEntityTypeConfiguration<CountryEntity>
    {
        public void Configure(EntityTypeBuilder<CountryEntity> builder)
        {
            var timeStamp = DateTime.Parse("2026-01-01T00:00:00");

            builder.HasData(
                new CountryEntity
                {
                    Id = 1,
                    Name = "United States",
                    CreatedAt = timeStamp,
                    CreatedBy = "System",
                    UpdatedAt = null,
                    UpdatedBy = null
                }
            );
        }
    }
}
