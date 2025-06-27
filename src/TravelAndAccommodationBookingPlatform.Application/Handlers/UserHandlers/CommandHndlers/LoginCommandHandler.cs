using AutoMapper;
using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Commands.UserCommands;
using TravelAndAccommodationBookingPlatform.Application.DTOs.UserDtos;
using TravelAndAccommodationBookingPlatform.Core.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Auth;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;

namespace TravelAndAccommodationBookingPlatform.Application.Handlers.UserHandlers.CommandHndlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IMapper mapper,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var AuthenticatedUser = await _userRepository.AuthenticateUserAsync(request.Username, request.Password);

            if (AuthenticatedUser == null)
            {
                throw new CredentialsNotValidException(UserMessages.InvalidCredentials);
            }

            var jwtToken = _jwtTokenGenerator.CreateTokenForUser(AuthenticatedUser);

            return _mapper.Map<LoginResponseDto>(jwtToken);
        }
    }
}
