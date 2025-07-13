using AutoMapper;
using TravelAndAccommodationBookingPlatform.Application.Commands.DiscountCommands;
using TravelAndAccommodationBookingPlatform.Application.Queries.DiscountQueries;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Discounts;

namespace TravelAndAccommodationBookingPlatform.WebAPI.Mapping
{
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            CreateMap<DiscountCreationRequestDto, CreateDiscountCommand>();
            CreateMap<GetDiscountsRequestDto, GetDiscountsQuery>()
                .ForMember(
                    dst => dst.OrderDirection,
                    opt => opt.MapFrom(src => src.OrderDirection));
        }
    }
}
