using AutoMapper;
using TravelAndAccommodationBookingPlatform.Application.Features.Booking.Commands.CreateBooking;
using TravelAndAccommodationBookingPlatform.Application.Features.Booking.Queries.GetBookings;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Bookings;
using TravelAndAccommodationBookingPlatform.WebAPI.Helpers;

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
                    opt => opt.MapFrom(src => MappingHelpers.MapOrderDirection(src.OrderDirection)));
        }
    }
}
