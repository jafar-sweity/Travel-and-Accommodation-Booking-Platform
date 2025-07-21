using AutoMapper;
using TravelAndAccommodationBookingPlatform.Application.Features.Rooms.Commands.CreateRoom;
using TravelAndAccommodationBookingPlatform.Application.Features.Rooms.Commands.UpdateRoom;
using TravelAndAccommodationBookingPlatform.Application.Features.Rooms.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Common;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Request;

namespace TravelAndAccommodationBookingPlatform.Application.Profiles
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<CreateRoomCommand, Room>();
            CreateMap<UpdateRoomCommand, Room>();
            CreateMap<RoomManagementDto, RoomManagementResponseDto>();
            CreateMap<PaginatedResult<RoomManagementDto>, PaginatedResult<RoomManagementResponseDto>>();
            CreateMap<Room, RoomGuestResponseDto>();
            CreateMap<PaginatedResult<RoomManagementDto>, PaginatedResult<RoomManagementResponseDto>>().ForMember(dst => dst.Items, options => options.MapFrom(src => src.Items));
            CreateMap<PaginatedResult<Room>, PaginatedResult<RoomGuestResponseDto>>().ForMember(dst => dst.Items, options => options.MapFrom(src => src.Items));
        }
    }
}
