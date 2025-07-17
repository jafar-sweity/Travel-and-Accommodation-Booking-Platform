using AutoMapper;
using TravelAndAccommodationBookingPlatform.Application.Features.RoomClasses.Commands.CreateRoomClass;
using TravelAndAccommodationBookingPlatform.Application.Features.RoomClasses.Commands.UpdateRoomClass;
using TravelAndAccommodationBookingPlatform.Application.Features.RoomClasses.Queries.GetRoomClassesManagement;
using TravelAndAccommodationBookingPlatform.Application.Features.RoomClasses.Queries.GetRoomClassGuest;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.RoomClasses;
using TravelAndAccommodationBookingPlatform.WebAPI.Helpers;

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
                    opt => opt.MapFrom(src => MappingHelpers.MapOrderDirection(src.OrderDirection)));

            CreateMap<GetRoomClassesRequestDto, GetRoomClassesManagementQuery>()
           .ForMember(
                 dst => dst.OrderDirection,
                    opt => opt.MapFrom(src => MappingHelpers.MapOrderDirection(src.OrderDirection)));

        }
    }
}
