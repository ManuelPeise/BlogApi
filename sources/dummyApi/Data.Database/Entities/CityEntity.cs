using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Database.Entities
{
    public class CityEntity: AEntityBase
    {
        public string PostalCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int? CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]
        public CountryEntity? Country { get; set; }
    }
}
