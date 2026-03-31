namespace Shared.Models
{
    public class AddressModel
    {
        public int AddressId { get; set; }
        public string Street { get; set; } = string.Empty;
        public string HouseNumber { get; set; } = string.Empty;
        public int CityId { get; set; }
        public string PostalCode { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public int CountryId { get; set; }
        public string CountryName { get; set; } = string.Empty;
    }
}
