using FluentValidation;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Cities;

namespace TravelAndAccommodationBookingPlatform.Application.Validators.Cities
{
    public class CityUpdateRequestDtoValidator : AbstractValidator<CityUpdateRequestDto>
    {
        public CityUpdateRequestDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required.")
                .MaximumLength(100).WithMessage("Country must not exceed 100 characters.");

            RuleFor(x => x.Region)
                .NotEmpty().WithMessage("Region is required.")
                .MaximumLength(100).WithMessage("Region must not exceed 100 characters.");

            RuleFor(x => x.PostOffice)
                .NotEmpty().WithMessage("PostOffice is required.")
                .MaximumLength(20).WithMessage("PostOffice must not exceed 20 characters.");
        }
    }
}