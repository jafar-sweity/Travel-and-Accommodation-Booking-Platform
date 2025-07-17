using AutoMapper;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Commands.CreateHotel;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Commands.UpdateHotel;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Queries.GetHotelFeaturedDeals;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Queries.GetHotelsManagement;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Queries.GetPublicHotels;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Queries.GetRecentlyVisitedHotels;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Queries.SearchHotels;
using TravelAndAccommodationBookingPlatform.Core.Enums;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Hotels;
using TravelAndAccommodationBookingPlatform.WebAPI.Helpers;

namespace TravelAndAccommodationBookingPlatform.WebAPI.Mapping
{
    public class HotelProfile : Profile
    {
        public HotelProfile()
        {
            CreateMap<GetHotelFeaturedDealsRequestDto, GetHotelFeaturedDealsQuery>();
            CreateMap<HotelCreationRequestDto, CreateHotelCommand>();
            CreateMap<HotelUpdateRequestDto, UpdateHotelCommand>();
            CreateMap<GetRecentlyVisitedHotelsRequestDto, GetRecentlyVisitedHotelsQuery>();
            CreateMap<GetHotelsRequestDto, GetHotelManagementQuery>()
              .ForMember(
                    dst => dst.OrderDirection,
                    opt => opt.MapFrom(src => src.OrderDirection));
            CreateMap<HotelSearchRequestDto, SearchHotelsQuery>()
              .ForMember(dst => dst.RoomTypes, opt => opt.MapFrom(src => src.RoomTypes ?? Enumerable.Empty<RoomType>()))
              .ForMember(
                    dst => dst.OrderDirection,
                    opt => opt.MapFrom(src => MappingHelpers.MapOrderDirection(src.OrderDirection)));

            CreateMap<GetHotelsPublicRequestDto, GetPublicHotelsQuery>();
        }
    }
}
