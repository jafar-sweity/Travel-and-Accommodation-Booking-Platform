﻿using FluentValidation;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Cities;

namespace TravelAndAccommodationBookingPlatform.WebAPI.Validators.City
{
    public class CityCreationRequestValidator : AbstractValidator<CityCreationRequestDto>
    {
        public CityCreationRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("City name is required.");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required.");

            RuleFor(x => x.Region)
                .NotEmpty().WithMessage("Region is required.");

            RuleFor(x => x.PostOffice)
                .NotEmpty().WithMessage("Post office is required.");
        }
    }
}