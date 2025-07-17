using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TravelAndAccommodationBookingPlatform.Application.Features.Users.Commands.Login;
using TravelAndAccommodationBookingPlatform.Application.Features.Users.Commands.Register;
using TravelAndAccommodationBookingPlatform.Application.Features.Users.DTOs;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Authentication;

namespace TravelAndAccommodationBookingPlatform.WebAPI.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    [ApiVersion("1.0")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public AuthenticationController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var loginCommand = _mapper.Map<LoginCommand>(loginRequestDto);
            var result = await _mediator.Send(loginCommand);
            return Ok(result);
        }

        [HttpPost("register-guest")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> RegisterUser(
            [FromBody] RegisterRequestDto registerRequestDto)
        {
            var registerCommand = new RegisterCommand { Role = "Guest" };
            _mapper.Map(registerRequestDto, registerCommand);
            await _mediator.Send(registerCommand);
            return NoContent();
        }


    }
}
