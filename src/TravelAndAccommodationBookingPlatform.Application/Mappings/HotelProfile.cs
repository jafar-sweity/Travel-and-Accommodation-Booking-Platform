using AutoMapper;
using TravelAndAccommodationBookingPlatform.Application.DTOs.HotelDtos;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Commands.CreateHotel;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Commands.UpdateHotel;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Common;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Request;

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
               .ForMember(dest => dest.SmallPreview, opt => opt.MapFrom(src => src.ThumbnailImage != null ? src.ThumbnailImage.ToString() : null))
               .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.CityName));
            CreateMap<PaginatedResult<HotelSearchDto>, PaginatedResult<HotelSearchResultResponseDto>>().ForMember(dst => dst.Items, options => options.MapFrom(src => src.Items));
            CreateMap<PaginatedResult<HotelManagementDto>, PaginatedResult<HotelManagementResponseDto>>().ForMember(dst => dst.Items, options => options.MapFrom(src => src.Items));

            CreateMap<RoomClass, HotelFeaturedDealResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Hotel.Name))
            .ForMember(dest => dest.RoomClassName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.Hotel.City.Name))
            .ForMember(dest => dest.Geolocation, opt => opt.MapFrom(src => src.Hotel.Geolocation))
            .ForMember(dest => dest.StarRating, opt => opt.MapFrom(src => src.Hotel.StarRating))
            .ForMember(dest => dest.NightlyRate, opt => opt.MapFrom(src => src.NightlyRate))
            .ForMember(dest => dest.DiscountPercentage, opt => opt.MapFrom(src => src.Discounts.First().Percentage))
            .ForMember(dest => dest.DiscountStartDate, opt => opt.MapFrom(src => src.Discounts.First().StartDate))
            .ForMember(dest => dest.DiscountEndDate, opt => opt.MapFrom(src => src.Discounts.First().EndDate))
            .ForMember(dest => dest.ReviewsRating, opt => opt.MapFrom(src => src.Hotel.ReviewsRating))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Hotel.BriefDescription))
            .ForMember(dest => dest.Thumbnail, opt => opt.MapFrom(src => src.Hotel.SmallPreview.Url ?? null));
        }
    }
}
