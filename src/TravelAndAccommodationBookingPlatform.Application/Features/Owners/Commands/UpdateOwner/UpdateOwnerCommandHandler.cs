using AutoMapper;
using MediatR;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.UnitOfWork;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Owners.Commands.UpdateOwner
{
    public class UpdateOwnerCommandHandler : IRequestHandler<UpdateOwnerCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;

        public UpdateOwnerCommandHandler(
            IUnitOfWork unitOfWork,
            IOwnerRepository ownerRepository,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _ownerRepository = ownerRepository;
            _mapper = mapper;
        }

        public async Task Handle(UpdateOwnerCommand request, CancellationToken cancellationToken)
        {
            var owner = await _ownerRepository.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException(OwnerMessages.OwnerNotFound);

            _mapper.Map(request, owner);
            await _ownerRepository.UpdateAsync(owner);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
