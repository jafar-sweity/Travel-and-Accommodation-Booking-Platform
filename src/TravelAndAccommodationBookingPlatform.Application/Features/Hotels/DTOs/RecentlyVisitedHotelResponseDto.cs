﻿namespace TravelAndAccommodationBookingPlatform.Application.Features.Hotels.DTOs
{
    public class RecentlyVisitedHotelResponseDto
    {
        public Guid Id { get; init; }
        public Guid BookingId { get; init; }
        public string Name { get; init; }
        public string City { get; init; }
        public string Country { get; init; }
        public int StarRating { get; init; }
        public double ReviewsRating { get; init; }
        public decimal TotalPrice { get; init; }
        public string? SmallPreview { get; init; }
        public DateOnly CheckInDate { get; init; }
        public DateOnly CheckOutDate { get; init; }
    }
}
