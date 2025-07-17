using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TravelAndAccommodationBookingPlatform.Application.Features.Discounts.Commands.CreateDiscount;
using TravelAndAccommodationBookingPlatform.Application.Features.Discounts.Commands.DeleteDiscount;
using TravelAndAccommodationBookingPlatform.Application.Features.Discounts.DTOs;
using TravelAndAccommodationBookingPlatform.Application.Features.Discounts.Queries.GetDiscountById;
using TravelAndAccommodationBookingPlatform.Application.Features.Discounts.Queries.GetDiscounts;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Discounts;

namespace TravelAndAccommodationBookingPlatform.WebAPI.Controllers
{
    [ApiController]
    [Route("api/room-classes/{roomClassId:guid}/discounts")]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin")]
    public class DiscountsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public DiscountsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DiscountResponseDto>> GetDiscount(Guid roomClassId, Guid id)
        {
            var query = new GetDiscountByIdQuery { RoomClassId = roomClassId, DiscountId = id };
            var discount = await _mediator.Send(query);
            return Ok(discount);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<DiscountResponseDto>>> GetDiscounts(Guid roomClassId, [FromQuery] GetDiscountsRequestDto getDiscountsRequestDto)
        {
            var query = new GetDiscountsQuery { RoomClassId = roomClassId };
            _mapper.Map(getDiscountsRequestDto, query);
            var result = await _mediator.Send(query);
            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(result.PaginationMetadata);
            return Ok(result.Items);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDiscount(Guid roomClassId, Guid id)
        {
            var command = new DeleteDiscountCommand { RoomClassId = roomClassId, DiscountId = id };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateDiscount(Guid roomClassId, [FromBody] DiscountCreationRequestDto discountCreationRequestDto)
        {
            var command = new CreateDiscountCommand { RoomClassId = roomClassId };
            _mapper.Map(discountCreationRequestDto, command);
            var createdDiscount = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetDiscount), new { roomClassId, id = createdDiscount.Id }, createdDiscount);
        }
    }
}
