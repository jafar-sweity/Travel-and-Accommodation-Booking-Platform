namespace TravelAndAccommodationBookingPlatform.Application.Features.Cities.DTOs
{
    public class TrendingCityResponseDto
    {
        public Guid Id { get; init; }
        public string? SmallPreview { get; init; }
        public string Name { get; init; }
        public string Country { get; init; }
    }
}
