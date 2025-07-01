namespace TravelAndAccommodationBookingPlatform.Application.DTOs.BookingDtos
{
    class BookingResponseDTO
    {
        public Guid Id { get; set; }
        public string HotelName { get; set; }
        public decimal TotalPrice { get; set; }
        public string? GuestRemarks { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime BookingDate { get; set; }

    }
}
