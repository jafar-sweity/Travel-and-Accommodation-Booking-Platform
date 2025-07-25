﻿using AutoMapper;
using TravelAndAccommodationBookingPlatform.Application.Features.Reviews.Commands.UpdateReview;
using TravelAndAccommodationBookingPlatform.Application.Features.Reviews.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Common;

namespace TravelAndAccommodationBookingPlatform.Application.Profiles
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReviewResponseDto>();
            CreateMap<UpdateReviewCommand, Review>();
            CreateMap<Review, ReviewResponseDto>();
            CreateMap<PaginatedResult<Review>, PaginatedResult<ReviewResponseDto>>().ForMember(dst => dst.Items, options => options.MapFrom(src => src.Items));
        }
    }
}
