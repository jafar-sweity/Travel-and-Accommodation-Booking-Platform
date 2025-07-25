﻿using MediatR;
using TravelAndAccommodationBookingPlatform.Core.Enums;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Commands.CreateHotel
{
    public class CreateHotelCommand : IRequest<Guid>
    {
        public Guid OwnerId { get; init; }
        public Guid CityId { get; init; }
        public string Name { get; init; }
        public int StarRating { get; init; }
        public string? Website { get; init; }
        public string? BriefDescription { get; init; }
        public string? FullDescription { get; init; }
        public string Geolocation { get; init; }
        public string PhoneNumber { get; init; }
        public HotelStatus Status { get; init; } = HotelStatus.Open;
    }
}
