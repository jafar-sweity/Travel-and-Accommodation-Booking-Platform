using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TravelAndAccommodationBookingPlatform.Application.Features.RoomClasses.Commands.AddGalleryToRoomClass;
using TravelAndAccommodationBookingPlatform.Application.Features.RoomClasses.Commands.CreateRoomClass;
using TravelAndAccommodationBookingPlatform.Application.Features.RoomClasses.Commands.DeleteRoomClass;
using TravelAndAccommodationBookingPlatform.Application.Features.RoomClasses.Commands.UpdateRoomClass;
using TravelAndAccommodationBookingPlatform.Application.Features.RoomClasses.DTOs;
using TravelAndAccommodationBookingPlatform.Application.Features.RoomClasses.Queries.GetRoomClassesManagement;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Images;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.RoomClasses;

namespace TravelAndAccommodationBookingPlatform.WebAPI.Controllers
{
    [ApiController]
    [Route("api/room-classes")]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin")]
    public class RoomClassesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public RoomClassesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<RoomClassManagementResponseDto>>> GetAllRoomClasses([FromQuery] GetRoomClassesRequestDto getRoomClassesRequestDto)
        {
            var query = _mapper.Map<GetRoomClassesManagementQuery>(getRoomClassesRequestDto);
            var result = await _mediator.Send(query);
            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(result.PaginationMetadata);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateRoomClass([FromBody] RoomClassCreationRequestDto createRoomClassRequestDto)
        {
            var command = _mapper.Map<CreateRoomClassCommand>(createRoomClassRequestDto);
            var createdRoomClassId = await _mediator.Send(command);
            return Created(string.Empty, createdRoomClassId);
        }

        [HttpPost("{id:guid}/gallery")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddGalleryToRoomClass(Guid id, [FromForm] ImageCreationRequestDto imageCreationRequestDto)
        {
            var command = new AddGalleryToRoomClassCommand
            {
                RoomClassId = id,
            };

            _mapper.Map(imageCreationRequestDto, command);

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateRoomClass(Guid id, [FromBody] RoomClassUpdateRequestDto roomClassUpdateRequestDto)
        {
            var command = new UpdateRoomClassCommand { RoomClassId = id };
            _mapper.Map(roomClassUpdateRequestDto, command);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteRoomClass(Guid id)
        {
            var command = new DeleteRoomClassCommand { RoomClassId = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
