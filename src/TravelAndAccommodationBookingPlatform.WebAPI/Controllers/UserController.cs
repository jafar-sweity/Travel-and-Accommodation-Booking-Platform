using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.DTOs;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Queries.GetRecentlyVisitedHotels;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Hotels;

namespace TravelAndAccommodationBookingPlatform.WebAPI.Controllers
{
    [ApiController]
    [Route("api/user/dashboard")]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Guest")]
    public class UserController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public UserController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("recently-visited-hotels")]
        [ProducesResponseType(typeof(IEnumerable<RecentlyVisitedHotelResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<RecentlyVisitedHotelResponseDto>>> GetRecentlyVisitedHotels([FromQuery] GetRecentlyVisitedHotelsRequestDto getRecentlyVisitedHotelsRequestDto)
        {
            var userIdClaim = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new ArgumentNullException());

            var query = new GetRecentlyVisitedHotelsQuery { GuestId = userIdClaim };
            _mapper.Map(getRecentlyVisitedHotelsRequestDto, query);
            var hotels = await _mediator.Send(query);
            return Ok(hotels);
        }
    }
}
