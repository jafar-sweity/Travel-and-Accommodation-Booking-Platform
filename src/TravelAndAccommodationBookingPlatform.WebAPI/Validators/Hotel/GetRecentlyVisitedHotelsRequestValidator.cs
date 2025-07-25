﻿using FluentValidation;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Hotels;

namespace TravelAndAccommodationBookingPlatform.WebAPI.Validators.Hotels
{
    public class GetRecentlyVisitedHotelsRequestValidator : AbstractValidator<GetRecentlyVisitedHotelsRequestDto>
    {
        public GetRecentlyVisitedHotelsRequestValidator()
        {
            RuleFor(x => x.Count).InclusiveBetween(1, 100).WithMessage("Count must be between 1 and 100.");
        }
    }
}