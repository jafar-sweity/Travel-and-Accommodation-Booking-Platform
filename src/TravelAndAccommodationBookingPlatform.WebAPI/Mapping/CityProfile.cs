using AutoMapper;
using TravelAndAccommodationBookingPlatform.Application.Features.Cities.Commands.CreateCity;
using TravelAndAccommodationBookingPlatform.Application.Features.Cities.Commands.UpdateCity;
using TravelAndAccommodationBookingPlatform.Application.Features.Cities.Queries.GetCitiesManagement;
using TravelAndAccommodationBookingPlatform.Application.Features.Cities.Queries.GetTrendingCities;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Cities;
using TravelAndAccommodationBookingPlatform.WebAPI.Helpers;

namespace TravelAndAccommodationBookingPlatform.WebAPI.Mapping
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<GetTrendingCitiesRequestDto, GetTrendingCitiesQuery>();
            CreateMap<CityCreationRequestDto, CreateCityCommand>();
            CreateMap<CityUpdateRequestDto, UpdateCityCommand>();
            CreateMap<GetCitiesRequestDto, GetCitiesManagementQuery>()
                .ForMember(
                    dst => dst.OrderDirection,
                    opt => opt.MapFrom(src => MappingHelpers.MapOrderDirection(src.OrderDirection)));
        }
    }
}
