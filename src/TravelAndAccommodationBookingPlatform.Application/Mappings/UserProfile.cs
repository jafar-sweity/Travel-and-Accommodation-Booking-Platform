using AutoMapper;
using TravelAndAccommodationBookingPlatform.Application.Features.Users.Commands.Register;
using TravelAndAccommodationBookingPlatform.Application.Features.Users.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Models.Auth;

namespace TravelAndAccommodationBookingPlatform.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<JwtAuthToken, LoginResponseDto>();
            CreateMap<RegisterCommand, User>();
        }
    }
}
