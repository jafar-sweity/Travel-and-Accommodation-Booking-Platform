using AutoMapper;
using TravelAndAccommodationBookingPlatform.Application.Commands.HotelCommands;
using TravelAndAccommodationBookingPlatform.Application.DTOs.HotelDtos;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Models;

namespace TravelAndAccommodationBookingPlatform.Application.Profiles
{
    class HotelProfile : Profile
    {
        public HotelProfile()
        {
            CreateMap<CreateHotelCommand, Hotel>();
            CreateMap<UpdateHotelCommand, Hotel>();
            CreateMap<HotelManagementDto, HotelManagementResponseDto>().ForMember(dst => dst.Owner, options => options.MapFrom(src => src.Owner));
            CreateMap<Hotel, HotelGuestResponseDto>()
            .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.City.Country))
            .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
            .ForMember(dest => dest.Thumbnail, opt => opt.MapFrom(src => src.SmallPreview != null ? src.SmallPreview.Url : null))
            .ForMember(dest => dest.Gallery, opt => opt.MapFrom(src => src.FullView.Select(image => image.Url)));

            CreateMap<HotelSearchDto, HotelSearchResultResponseDto>()
               .ForMember(dest => dest.BriefDescription, opt => opt.MapFrom(src => src.Description))
               .ForMember(dest => dest.SmallPreview, opt => opt.MapFrom(src => src.ThumbnailImage != null ? src.ThumbnailImage.ToString() : null));
            CreateMap<PaginatedResult<HotelSearchDto>, PaginatedResult<HotelSearchResultResponseDto>>().ForMember(dst => dst.Items, options => options.MapFrom(src => src.Items));
            CreateMap<PaginatedResult<HotelManagementDto>, PaginatedResult<HotelManagementResponseDto>>().ForMember(dst => dst.Items, options => options.MapFrom(src => src.Items));
        }
    }
}
