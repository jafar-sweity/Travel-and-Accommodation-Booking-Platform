using AutoMapper;
using MediatR;
using TravelAndAccommodationBookingPlatform.Application.DTOs.OwnerDtos;
using TravelAndAccommodationBookingPlatform.Application.Queries.OwnerQueries;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;


namespace TravelAndAccommodationBookingPlatform.Application.Handlers.OwnerHandlers.QueryHandlers
{
    public class GetOwnerByIdQueryHandler : IRequestHandler<GetOwnerByIdQuery, OwnerResponseDto>
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;

        public GetOwnerByIdQueryHandler(
            IOwnerRepository ownerRepository,
            IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
        }

        public async Task<OwnerResponseDto> Handle(GetOwnerByIdQuery request, CancellationToken cancellationToken)
        {
            var owner = await _ownerRepository.GetByIdAsync(request.Id) ?? throw new NotFoundException(OwnerMessages.OwnerNotFound);

            return _mapper.Map<OwnerResponseDto>(owner);
        }
    }
}
