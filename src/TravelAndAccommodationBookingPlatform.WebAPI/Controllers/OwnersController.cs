using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TravelAndAccommodationBookingPlatform.Application.Features.Owners.Commands.CreateOwner;
using TravelAndAccommodationBookingPlatform.Application.Features.Owners.Commands.UpdateOwner;
using TravelAndAccommodationBookingPlatform.Application.Features.Owners.DTOs;
using TravelAndAccommodationBookingPlatform.Application.Features.Owners.Queries.GetOwnerById;
using TravelAndAccommodationBookingPlatform.Application.Features.Owners.Queries.GetOwners;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Common;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Owners;

namespace TravelAndAccommodationBookingPlatform.WebAPI.Controllers
{
    [ApiController]
    [Route("api/owners")]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin")]
    public class OwnersController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public OwnersController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<PaginatedResult<OwnerResponseDto>>> GetOwners([FromQuery] GetOwnersRequestDto getOwnersRequestDto)
        {
            var query = _mapper.Map<GetOwnersQuery>(getOwnersRequestDto);
            var result = await _mediator.Send(query);
            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(result.PaginationMetadata);
            return Ok(result.Items);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOwner(Guid id, [FromBody] OwnerUpdateRequestDto ownerUpdateRequestDto)
        {
            var command = new UpdateOwnerCommand { OwnerId = id };

            _mapper.Map(ownerUpdateRequestDto, command);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OwnerResponseDto>> GetOwner(Guid id)
        {
            var query = new GetOwnerByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateOwner([FromBody] OwnerCreationRequestDto ownerCreationRequestDto)
        {
            var command = _mapper.Map<CreateOwnerCommand>(ownerCreationRequestDto);
            var createdOwner = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetOwner), new { id = createdOwner.Id }, createdOwner);
        }
    }
}
