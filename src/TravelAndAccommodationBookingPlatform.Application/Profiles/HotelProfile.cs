using AutoMapper;
using TravelAndAccommodationBookingPlatform.Application.Commands.HotelCommands;
using TravelAndAccommodationBookingPlatform.Core.Entities;

namespace TravelAndAccommodationBookingPlatform.Application.Profiles
{
    class HotelProfile : Profile
    {
        public HotelProfile()
        {
            CreateMap<CreateHotelCommand, Hotel>();
        }
    }
}
