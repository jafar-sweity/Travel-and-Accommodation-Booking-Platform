namespace TravelAndAccommodationBookingPlatform.Application.DTOs.CityDtos
{
    public class CityResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PostOffice { get; set; } = string.Empty;
    }
}
