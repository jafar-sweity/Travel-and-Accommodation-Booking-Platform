using FluentValidation;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Cities;

namespace TravelAndAccommodationBookingPlatform.Application.Validators.Cities
{
    public class GetCitiesRequestDtoValidator : AbstractValidator<GetCitiesRequestDto>
    {
        public GetCitiesRequestDtoValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("PageNumber must be at least 1.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 20)
                .WithMessage("PageSize must be between 1 and 20.");

            RuleFor(x => x.SortColumn)
                .Must(IsValidSortColumn)
                .WithMessage($"Sort Column must be empty or one of the following: {string.Join(", ", ValidSortColumns)}.");

            RuleFor(x => x.OrderDirection)
                .Must(x => string.IsNullOrEmpty(x) || x.Equals("asc", System.StringComparison.OrdinalIgnoreCase) || x.Equals("desc", System.StringComparison.OrdinalIgnoreCase))
                .WithMessage("OrderDirection must be either 'asc', 'desc', or empty.");

            RuleFor(x => x.Search)
                .MaximumLength(100)
                .When(x => !string.IsNullOrEmpty(x.Search))
                .WithMessage("Search term must not exceed 100 characters.");
        }

        private static readonly string[] ValidSortColumns = { "id", "Name", "Country", "PostOffice" };
        private static bool IsValidSortColumn(string? sortColumn) => string.IsNullOrWhiteSpace(sortColumn) || ValidSortColumns.Any(col => string.Equals(col, sortColumn, StringComparison.OrdinalIgnoreCase));
    }
}