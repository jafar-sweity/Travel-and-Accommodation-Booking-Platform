﻿using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Users.DTOs;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Users.Commands.Login
{
    public class LoginCommand : IRequest<LoginResponseDto>
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
