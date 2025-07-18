﻿using MediatR;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Owners.Commands.UpdateOwner
{
    public class UpdateOwnerCommand : IRequest
    {
        public Guid OwnerId;

        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
