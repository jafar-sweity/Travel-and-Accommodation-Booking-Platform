using AutoMapper;
using TravelAndAccommodationBookingPlatform.Application.Features.Users.Commands.Login;
using TravelAndAccommodationBookingPlatform.Application.Features.Users.Commands.Register;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Authentication;

namespace TravelAndAccommodationBookingPlatform.WebAPI.Mapping
{
    public class AuthenticationProfile : Profile
    {
        public AuthenticationProfile()
        {
            CreateMap<LoginRequestDto, LoginCommand>();
            CreateMap<RegisterRequestDto, RegisterCommand>();
        }
    }
}
