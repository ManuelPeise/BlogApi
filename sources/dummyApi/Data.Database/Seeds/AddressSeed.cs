using Data.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Database.Seeds
{
    internal class AddressSeed : IEntityTypeConfiguration<AddressEntity>
    {
        public void Configure(EntityTypeBuilder<AddressEntity> builder)
        {
            var timeStamp = DateTime.Parse("2026-01-01T00:00:00");

            builder.HasData(
                new AddressEntity
                {
                    Id = 1,
                    Street = "Main Street",
                    HouseNumber = "123",
                    CityId = 1,
                    CreatedAt = timeStamp,
                    CreatedBy = "System",
                    UpdatedBy = null,
                    UpdatedAt = null
                }
            );
        }
    }
}
