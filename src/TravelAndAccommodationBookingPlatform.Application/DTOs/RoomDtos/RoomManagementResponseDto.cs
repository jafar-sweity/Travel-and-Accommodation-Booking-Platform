namespace TravelAndAccommodationBookingPlatform.Application.DTOs.RoomDtos
{
    public class RoomManagementResponseDto
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public bool IsAvailable { get; set; }
        public Guid RoomClassID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
