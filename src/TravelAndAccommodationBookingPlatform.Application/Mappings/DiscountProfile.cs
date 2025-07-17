using AutoMapper;
using TravelAndAccommodationBookingPlatform.Application.Features.Discounts.Commands.CreateDiscount;
using TravelAndAccommodationBookingPlatform.Application.Features.Discounts.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Models;

namespace TravelAndAccommodationBookingPlatform.Application.Profiles
{
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            CreateMap<CreateDiscountCommand, Discount>();
            CreateMap<Discount, DiscountResponseDto>();
            CreateMap<PaginatedResult<Discount>, PaginatedResult<DiscountResponseDto>>().ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        }
    }
}
