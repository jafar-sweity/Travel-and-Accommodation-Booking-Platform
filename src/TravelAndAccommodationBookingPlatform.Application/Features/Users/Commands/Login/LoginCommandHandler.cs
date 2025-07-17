using AutoMapper;
using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Users.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Constants.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Auth;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Users.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IPasswordHashService _passwordHashService;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IMapper mapper,
            IJwtTokenGenerator jwtTokenGenerator,
            IPasswordHashService passwordHashService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtTokenGenerator = jwtTokenGenerator;
            _passwordHashService = passwordHashService;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByUsernameAsync(request.Username);

            if (user == null || !_passwordHashService.VerifyPassword(request.Password, user.HashedPassword))
            {
                throw new CredentialsNotValidException(UserMessages.InvalidCredentials);
            }

            var jwtToken = _jwtTokenGenerator.CreateTokenForUser(user);

            return _mapper.Map<LoginResponseDto>(jwtToken);
        }
    }
}
