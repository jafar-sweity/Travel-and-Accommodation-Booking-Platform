﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TravelAndAccommodationBookingPlatform.Application.Features.Cities.Commands.AddCitySmallPreviewImage;
using TravelAndAccommodationBookingPlatform.Application.Features.Cities.Commands.CreateCity;
using TravelAndAccommodationBookingPlatform.Application.Features.Cities.Commands.DeleteCity;
using TravelAndAccommodationBookingPlatform.Application.Features.Cities.Commands.UpdateCity;
using TravelAndAccommodationBookingPlatform.Application.Features.Cities.DTOs;
using TravelAndAccommodationBookingPlatform.Application.Features.Cities.Queries.GetCitiesManagement;
using TravelAndAccommodationBookingPlatform.Application.Features.Cities.Queries.GetTrendingCities;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Cities;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Images;

namespace TravelAndAccommodationBookingPlatform.WebAPI.Controllers
{
    [ApiController]
    [Route("api/cities")]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin")]
    public class CitiesController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public CitiesController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("trending")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<TrendingCityResponseDto>>> GetTrendingCities([FromQuery] GetTrendingCitiesRequestDto getTrendingCitiesRequestDto)
        {
            var query = _mapper.Map<GetTrendingCitiesQuery>(getTrendingCitiesRequestDto);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityManagementResponseDto>>> GetCitiesManagement([FromQuery] GetCitiesRequestDto GetcitiesRequestDto)
        {
            var query = _mapper.Map<GetCitiesManagementQuery>(GetcitiesRequestDto);
            var result = await _mediator.Send(query);
            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(result.PaginationMetadata);
            return Ok(result.Items);
        }

        /// <returns>Returns the location of the created city.</returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost]
        public async Task<IActionResult> CreateCity([FromBody] CityCreationRequestDto cityCreationRequestDto)
        {
            var command = _mapper.Map<CreateCityCommand>(cityCreationRequestDto);
            var createdCity = await _mediator.Send(command);
            return Created(string.Empty, createdCity);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id:guid}/thumbnail")]
        public async Task<IActionResult> AddCitySmallPreviewImage(Guid id, [FromForm] ImageCreationRequestDto imageCreationRequestDto)
        {
            var command = new AddCitySmallPreviewImageCommand
            {
                CityId = id,

            };
            _mapper.Map(imageCreationRequestDto, command);
            await _mediator.Send(command);
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCity(Guid id, [FromBody] CityUpdateRequestDto cityUpdateRequestDto)
        {
            var command = new UpdateCityCommand { CityId = id };
            _mapper.Map(cityUpdateRequestDto, command);

            await _mediator.Send(command);
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCity(Guid id)
        {
            var command = new DeleteCityCommand
            {
                CityId = id
            };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
