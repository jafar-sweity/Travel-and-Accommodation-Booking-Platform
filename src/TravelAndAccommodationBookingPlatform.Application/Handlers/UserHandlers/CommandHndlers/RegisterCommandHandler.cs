using AutoMapper;
using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Commands.UserCommands;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Auth;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.UnitOfWork;

namespace TravelAndAccommodationBookingPlatform.Application.Handlers.UserHandlers.CommandHndlers
{
    class RegisterCommandHandler : IRequestHandler<RegisterCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;
        private readonly IPasswordHashService _passwordHashService;

        public RegisterCommandHandler(
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IMapper mapper,
            IRoleRepository roleRepository,
            IPasswordHashService passwordHashService)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _mapper = mapper;
            _roleRepository = roleRepository;
            _passwordHashService = passwordHashService;
        }

        public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.UserExistsByEmailAsync(request.Email))
                throw new UserWithEmailAlreadyExistsException(UserMessages.EmailAlreadyExists);

            var role = await _roleRepository.GetRoleByNameAsync(request.Role) ?? throw new InvalidRoleException(UserMessages.InvalidRole);

            var user = _mapper.Map<User>(request);

            user.HashedPassword = _passwordHashService.HashPassword(request.Password);

            user.Roles.Add(role);

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

