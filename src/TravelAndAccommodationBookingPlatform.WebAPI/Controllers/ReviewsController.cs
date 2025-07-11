using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TravelAndAccommodationBookingPlatform.Application.Commands.DiscountCommands;
using TravelAndAccommodationBookingPlatform.Application.Commands.ReviewCommands;
using TravelAndAccommodationBookingPlatform.Application.DTOs.ReviewDtos;
using TravelAndAccommodationBookingPlatform.Application.Queries.ReviewQueries;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Reviews;

namespace TravelAndAccommodationBookingPlatform.WebAPI.Controllers
{
    [ApiController]
    [Route("api/hotels/{hotelId:guid}/reviews")]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Guest")]
    public class ReviewsController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public ReviewsController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReviewResponseDto>> GetReview(Guid hotelId, Guid id)
        {
            var query = new GetReviewByIdQuery { HotelId = hotelId, ReviewId = id };
            var review = await _mediator.Send(query);
            return Ok(review);
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ReviewResponseDto>>> GetReviews(Guid hotelId, [FromQuery] GetReviewsRequestDto getReviewsGetRequestDto)
        {
            var query = new GetReviewsByHotelIdQuery { HotelId = hotelId };
            _mapper.Map(getReviewsGetRequestDto, query);
            var result = await _mediator.Send(query);
            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(result.PaginationMetadata);
            return Ok(result.Items);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateReview(Guid hotelId, [FromBody] ReviewCreationRequestDto reviewCreationRequestDto)
        {
            var command = new CreateReviewCommand { HotelId = hotelId };
            _mapper.Map(reviewCreationRequestDto, command);
            var createdReview = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetReview), new { hotelId, id = createdReview.Id }, createdReview);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateReview(Guid hotelId, Guid id, [FromBody] ReviewUpdateRequestDto reviewUpdateRequestDto)
        {
            var command = new UpdateReviewCommand { HotelId = hotelId, ReviewId = id };
            _mapper.Map(reviewUpdateRequestDto, command);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteReview(Guid hotelId, Guid id)
        {
            var command = new DeleteReviewCommand { HotelId = hotelId, ReviewId = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
