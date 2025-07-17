using AutoMapper;
using TravelAndAccommodationBookingPlatform.Application.Features.Rooms.Commands.CreateRoom;
using TravelAndAccommodationBookingPlatform.Application.Features.Rooms.Queries.GetGuestRoomsByClassId;
using TravelAndAccommodationBookingPlatform.Application.Features.Rooms.Queries.GetRoomsManagement;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Rooms;
using TravelAndAccommodationBookingPlatform.WebAPI.Helpers;

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
                    opt => opt.MapFrom(src => MappingHelpers.MapOrderDirection(src.OrderDirection)));
            CreateMap<GetRoomsGuestsRequestDto, GuestRoomsByClassIdQuery>();
        }
    }
}
