using AutoMapper;
using TravelAndAccommodationBookingPlatform.Application.Features.Cities.Commands.CreateCity;
using TravelAndAccommodationBookingPlatform.Application.Features.Cities.Commands.UpdateCity;
using TravelAndAccommodationBookingPlatform.Application.Features.Cities.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Models;

namespace TravelAndAccommodationBookingPlatform.Application.Profiles
{
    class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<CreateCityCommand, City>();
            CreateMap<City, CityResponseDto>();
            CreateMap<UpdateCityCommand, City>();
            CreateMap<PaginatedResult<CityManagementDto>, PaginatedResult<CityManagementResponseDto>>().
              ForMember(dst => dst.Items, options => options.MapFrom(src => src.Items)); ;
            CreateMap<CityManagementDto, CityManagementResponseDto>();
            CreateMap<City, TrendingCityResponseDto>()
              .ForMember(dst => dst.SmallPreview, options => options.MapFrom(src => src.SmallPreview != null ? src.SmallPreview.Url : null));
        }
    }
}
