using Data.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Database.Seeds
{
    internal class CitySeed : IEntityTypeConfiguration<CityEntity>
    {
        void IEntityTypeConfiguration<CityEntity>.Configure(EntityTypeBuilder<CityEntity> builder)
        {
            var timeStamp = DateTime.Parse("2026-01-01T00:00:00");

            builder.HasData(
                new CityEntity
                {
                    Id = 1,
                    CountryId = 1,
                    Name = "New York",
                    PostalCode = "10001",
                    CreatedAt = timeStamp,
                    CreatedBy = "System",
                    UpdatedAt = null,
                    UpdatedBy = null
                }
            );
        }
    }
}
