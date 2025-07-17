using AutoMapper;
using TravelAndAccommodationBookingPlatform.Application.Features.Booking.DTOs;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Models;

namespace TravelAndAccommodationBookingPlatform.Application.Profiles
{
    class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<PaginatedResult<Booking>, PaginatedResult<BookingResponseDTO>>();
            CreateMap<Booking, BookingResponseDTO>()
                .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.Name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
                .ForMember(dest => dest.CheckInDate, opt => opt.MapFrom(src => src.CheckInDate.ToDateTime(TimeOnly.MinValue)))
                .ForMember(dest => dest.CheckOutDate, opt => opt.MapFrom(src => src.CheckOutDate.ToDateTime(TimeOnly.MinValue)))
                .ForMember(dest => dest.BookingDate, opt => opt.MapFrom(src => src.BookingDate.ToDateTime(TimeOnly.MinValue)))
                .ForMember(dest => dest.GuestRemarks, opt => opt.MapFrom(src => src.GuestRemarks));


            CreateMap<PaginatedResult<Booking>, PaginatedResult<BookingResponseDTO>>();

            CreateMap<Booking, RecentlyVisitedHotelResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Hotel.Id))
                .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Hotel.Name))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Hotel.City.Name))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Hotel.City.Country))
                .ForMember(dest => dest.StarRating, opt => opt.MapFrom(src => src.Hotel.StarRating))
                .ForMember(dest => dest.ReviewsRating, opt => opt.MapFrom(src => src.Hotel.ReviewsRating))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
                .ForMember(dest => dest.SmallPreview, opt => opt.MapFrom(src => src.Hotel.SmallPreview != null ? src.Hotel.SmallPreview.Url : null))
                .ForMember(dest => dest.CheckInDate, opt => opt.MapFrom(src => src.CheckInDate))
                .ForMember(dest => dest.CheckOutDate, opt => opt.MapFrom(src => src.CheckOutDate));
        }
    }
}
