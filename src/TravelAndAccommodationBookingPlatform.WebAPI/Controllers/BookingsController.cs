using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TravelAndAccommodationBookingPlatform.Application.Commands.BookingCommands;
using TravelAndAccommodationBookingPlatform.Application.DTOs.BookingDtos;
using TravelAndAccommodationBookingPlatform.Application.Queries.BookingQueries;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Bookings;

namespace TravelAndAccommodationBookingPlatform.WebAPI.Controllers
{
    [ApiController]
    [Route("api/user/bookings")]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Guest")]
    public class BookingsController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public BookingsController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<BookingResponseDTO>>> GetBookings([FromQuery] GetBookingsRequestDto getBookingsRequestDto)
        {
            var query = _mapper.Map<GetBookingQuery>(getBookingsRequestDto);
            var bookings = await _mediator.Send(query);
            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(bookings.PaginationMetadata);
            return Ok(bookings.Items);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingResponseDTO>> GetBooking(Guid id)
        {
            var query = new GetBookingByIdQuery
            {
                BookingId = id
            };

            var booking = await _mediator.Send(query);
            return Ok(booking);
        }

        [HttpGet("{id:guid}/invoice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<FileResult> GetInvoicePdf(Guid id)
        {
            var query = new RetrieveInvoicePdfQuery { BookingId = id };
            var pdfData = await _mediator.Send(query);
            return File(pdfData, "application/pdf", $"Invoice-{id}.pdf");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateBooking([FromBody] BookingCreationRequestDto bookingCreationRequestDto)
        {
            var command = _mapper.Map<CreateBookingCommand>(bookingCreationRequestDto);
            var createdBooking = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetBooking), new { id = createdBooking.Id }, createdBooking);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteBooking(Guid id)
        {
            var command = new DeleteBookingCommand { BookingId = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }

}
