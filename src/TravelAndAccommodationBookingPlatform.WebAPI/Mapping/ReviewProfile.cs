using AutoMapper;
using TravelAndAccommodationBookingPlatform.Application.Features.Reviews.Commands.CreateReview;
using TravelAndAccommodationBookingPlatform.Application.Features.Reviews.Commands.UpdateReview;
using TravelAndAccommodationBookingPlatform.Application.Features.Reviews.Queries.GetReviewsByHotelId;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Reviews;
using TravelAndAccommodationBookingPlatform.WebAPI.Helpers;

namespace TravelAndAccommodationBookingPlatform.WebAPI.Mapping
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<ReviewUpdateRequestDto, UpdateReviewCommand>();
            CreateMap<GetReviewsRequestDto, GetReviewsByHotelIdQuery>()
                .ForMember(
                    dst => dst.OrderDirection,
                    opt => opt.MapFrom(src => MappingHelpers.MapOrderDirection(src.OrderDirection)));
            CreateMap<ReviewCreationRequestDto, CreateReviewCommand>();
        }
    }
}
