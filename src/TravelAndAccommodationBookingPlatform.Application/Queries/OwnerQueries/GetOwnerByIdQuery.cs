using MediatR;
using TravelAndAccommodationBookingPlatform.Application.DTOs.OwnerDtos;

namespace TravelAndAccommodationBookingPlatform.Application.Queries.OwnerQueries
{
    public class GetOwnerByIdQuery : IRequest<OwnerResponseDto>
    {
        public Guid Id { get; set; }
    }
}
