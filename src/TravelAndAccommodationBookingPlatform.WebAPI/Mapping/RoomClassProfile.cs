using AutoMapper;
using TravelAndAccommodationBookingPlatform.Application.Commands.RoomClassCommands;
using TravelAndAccommodationBookingPlatform.Application.Queries.RoomClassQueries;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.RoomClasses;

namespace TravelAndAccommodationBookingPlatform.WebAPI.Mapping
{
    public class RoomClassProfile : Profile
    {
        public RoomClassProfile()
        {
            CreateMap<RoomClassCreationRequestDto, CreateRoomClassCommand>();

            CreateMap<RoomClassUpdateRequestDto, UpdateRoomClassCommand>();

            CreateMap<GetRoomClassesGuestRequestDto, GetRoomClassGuestQuery>()
            .ForMember(
                 dst => dst.OrderDirection,
                 opt => opt.MapFrom(src => src.OrderDirection));

            CreateMap<GetRoomClassesRequestDto, GetRoomClassesManagementQuery>()
           .ForMember(
                 dst => dst.OrderDirection,
                 opt => opt.MapFrom(src => src.OrderDirection));

        }
    }
}
