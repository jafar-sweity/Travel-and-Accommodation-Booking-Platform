using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Owners.DTOs;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Owners.Commands.CreateOwner
{
    public class CreateOwnerCommand : IRequest<OwnerResponseDto>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
