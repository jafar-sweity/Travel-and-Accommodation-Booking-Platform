using FluentValidation;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Cities;

namespace TravelAndAccommodationBookingPlatform.Application.Validators.Cities
{
    public class GetTrendingCitiesRequestDtoValidator : AbstractValidator<GetTrendingCitiesRequestDto>
    {
        public GetTrendingCitiesRequestDtoValidator()
        {
            RuleFor(x => x.Count)
            .GreaterThan(1)
            .WithMessage("Count Must Be grater than 1");
        }
    }
}