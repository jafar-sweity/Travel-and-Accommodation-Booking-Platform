using AutoMapper;
using TravelAndAccommodationBookingPlatform.Application.Commands.RoomCommands;
using TravelAndAccommodationBookingPlatform.Application.Queries.RoomQueries;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Rooms;

namespace TravelAndAccommodationBookingPlatform.WebAPI.Mapping
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<RoomCreationRequestDto, CreateRoomCommand>();
            CreateMap<GetRoomsRequestDto, GetRoomManagementQuery>()
              .ForMember(
                    dst => dst.OrderDirection,
                    opt => opt.MapFrom(src => src.OrderDirection));
            CreateMap<GetRoomsGuestsRequestDto, GuestRoomsByClassIdQuery>();
        }
    }
}
