namespace TravelAndAccommodationBookingPlatform.Application.DTOs.ReviewDtos
{
    public class ReviewResponseDto
    {
        public Guid Id { get; set; }
        public string GuestName { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
