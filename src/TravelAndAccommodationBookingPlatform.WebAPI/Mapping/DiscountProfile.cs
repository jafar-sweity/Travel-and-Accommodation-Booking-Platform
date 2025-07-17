using AutoMapper;
using TravelAndAccommodationBookingPlatform.Application.Features.Discounts.Commands.CreateDiscount;
using TravelAndAccommodationBookingPlatform.Application.Features.Discounts.Queries.GetDiscounts;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Discounts;
using TravelAndAccommodationBookingPlatform.WebAPI.Helpers;

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
                    opt => opt.MapFrom(src => MappingHelpers.MapOrderDirection(src.OrderDirection)));
        }
    }
}
