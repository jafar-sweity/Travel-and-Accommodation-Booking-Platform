using AutoMapper;
using TravelAndAccommodationBookingPlatform.Application.Commands.BookingCommands;
using TravelAndAccommodationBookingPlatform.Application.Queries.BookingQueries;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Bookings;

namespace TravelAndAccommodationBookingPlatform.WebAPI.Mapping
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<BookingCreationRequestDto, CreateBookingCommand>();
            CreateMap<GetBookingsRequestDto, GetBookingQuery>()
                .ForMember(
                    dst => dst.OrderDirection,
                    opt => opt.MapFrom(src => src.OrderDirection));
        }
    }
}
