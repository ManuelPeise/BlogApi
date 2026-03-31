using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Database.Entities
{
    public class AddressEntity: AEntityBase
    {
        public string Street { get; set; } = string.Empty;
        public string HouseNumber { get; set; } = string.Empty;
        public int? CityId { get; set; }
        [ForeignKey(nameof(CityId))]
        public CityEntity? City { get; set; } = null!;
    }
}
