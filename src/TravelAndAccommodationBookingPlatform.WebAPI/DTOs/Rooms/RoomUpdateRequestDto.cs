namespace TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Rooms
{
    public class RoomUpdateRequestDto
    {
        public string? Number { get; init; }
        public string? Description { get; init; }
        public decimal? PricePerNight { get; init; }
        public int? MaxGuests { get; init; }
        public bool? IsAvailable { get; init; }
    }
}
