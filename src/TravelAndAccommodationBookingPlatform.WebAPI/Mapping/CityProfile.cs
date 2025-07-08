using AutoMapper;
using TravelAndAccommodationBookingPlatform.Application.Commands.CityCommands;
using TravelAndAccommodationBookingPlatform.Application.Queries.CityQueries;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Cities;

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
                    opt => opt.MapFrom(src => src.OrderDirection));
        }
    }
}
