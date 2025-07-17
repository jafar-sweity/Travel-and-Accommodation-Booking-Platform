using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TravelAndAccommodationBookingPlatform.Application.Features.Rooms.Commands.CreateRoom;
using TravelAndAccommodationBookingPlatform.Application.Features.Rooms.Commands.DeleteRoom;
using TravelAndAccommodationBookingPlatform.Application.Features.Rooms.Commands.UpdateRoom;
using TravelAndAccommodationBookingPlatform.Application.Features.Rooms.DTOs;
using TravelAndAccommodationBookingPlatform.Application.Features.Rooms.Queries.GetRoomsManagement;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Rooms;

namespace TravelAndAccommodationBookingPlatform.WebAPI.Controllers
{
    [ApiController]
    [Route("api/room-classes/{roomClassId:guid}/rooms")]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin")]
    public class RoomsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public RoomsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("availableRooms")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<RoomManagementResponseDto>>> GetRoomsGuests(Guid roomClassId, [FromQuery] GetRoomsRequestDto request)
        {
            var query = new GetRoomManagementQuery { RoomClassId = roomClassId };
            _mapper.Map(request, query);
            var result = await _mediator.Send(query);
            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(result.PaginationMetadata);
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<RoomManagementResponseDto>>> GetRoomsManagement(Guid roomClassId, [FromQuery] GetRoomsRequestDto getRoomsRequestDto)
        {
            var query = new GetRoomManagementQuery { RoomClassId = roomClassId };
            _mapper.Map(getRoomsRequestDto, query);
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
        public async Task<IActionResult> CreateRoom(Guid roomClassId, [FromBody] RoomCreationRequestDto roomCreationRequestDto)
        {
            var command = _mapper.Map<CreateRoomCommand>(roomCreationRequestDto);
            command.RoomClassId = roomClassId;
            await _mediator.Send(command);
            return Created();
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateRoom(Guid roomClassId, Guid id, [FromBody] RoomUpdateRequestDto roomUpdateRequestDto)
        {
            var command = new UpdateRoomCommand { RoomClassId = roomClassId, RoomId = id };
            _mapper.Map(roomUpdateRequestDto, command);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteRoom(Guid roomClassId, Guid id)
        {
            var command = new DeleteRoomCommand { RoomClassId = roomClassId, RoomId = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
