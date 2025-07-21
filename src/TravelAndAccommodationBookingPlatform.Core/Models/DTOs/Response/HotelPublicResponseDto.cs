namespace TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Response
{
    public class HotelPublicResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? BriefDescription { get; set; }
        public double ReviewsRating { get; set; }
        public int StarRating { get; set; }
        public decimal NightlyRate { get; set; }
        public string? SmallPreview { get; set; }
        public string? CityName { get; set; }
    }
}
