namespace TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Common
{
    public record PaginatedResult<TItem>(IEnumerable<TItem> Items, PaginationMetadata PaginationMetadata);
}
