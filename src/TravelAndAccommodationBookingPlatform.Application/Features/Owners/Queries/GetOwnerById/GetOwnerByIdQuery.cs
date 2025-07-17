using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Owners.DTOs;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Owners.Queries.GetOwnerById
{
    public class GetOwnerByIdQuery : IRequest<OwnerResponseDto>
    {
        public Guid Id { get; set; }
    }
}
