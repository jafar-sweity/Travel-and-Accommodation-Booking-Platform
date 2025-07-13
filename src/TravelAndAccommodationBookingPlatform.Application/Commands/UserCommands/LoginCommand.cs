using MediatR;
using TravelAndAccommodationBookingPlatform.Application.DTOs.UserDtos;

namespace TravelAndAccommodationBookingPlatform.Application.Commands.UserCommands
{
    public class LoginCommand : IRequest<LoginResponseDto>
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
