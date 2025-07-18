﻿using MediatR;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Users.Commands.Register
{
    public class RegisterCommand : IRequest
    {
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Password { get; init; }
        public string Email { get; init; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; init; }
    }
}
