using FluentValidation;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Bookings;

namespace TravelAndAccommodationBookingPlatform.Application.Validators.Bookings
{
    public class GetBookingsRequestDtoValidator : AbstractValidator<GetBookingsRequestDto>
    {
        public GetBookingsRequestDtoValidator()
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

        }

        private static readonly string[] ValidSortColumns = { "id", "checkInDate", "checkOutDate", "bookingDate" };
        private static bool IsValidSortColumn(string? sortColumn) => string.IsNullOrWhiteSpace(sortColumn) || ValidSortColumns.Any(col => string.Equals(col, sortColumn, StringComparison.OrdinalIgnoreCase));
    }
}