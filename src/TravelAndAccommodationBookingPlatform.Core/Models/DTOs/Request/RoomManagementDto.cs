namespace TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Request
{
    public class RoomManagementDto
    {
        public Guid Id { get; set; }
        public Guid RoomClassId { get; set; }
        public bool IsAvailable { get; set; }
        public string Number { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
