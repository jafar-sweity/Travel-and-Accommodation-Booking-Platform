﻿namespace TravelAndAccommodationBookingPlatform.Core.Models
{
    public class HotelSearchDto
    {
        public Guid Id { get; set; }
        public string ThumbnailImage { get; set; }
        public string Name { get; set; }
        public int StarRating { get; set; }
        public string? CityName { get; init; }
        public double ReviewsRating { get; set; }
        public decimal NightlyRate { get; set; }
        public string? Description { get; set; }
    }
}
