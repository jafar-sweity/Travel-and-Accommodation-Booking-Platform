﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TravelAndAccommodationBookingPlatform.Application.DTOs.HotelDtos;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Commands.AddGalleryToHotel;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Commands.AddHotelThumbnail;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Commands.CreateHotel;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Commands.DeleteHotel;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Commands.UpdateHotel;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.DTOs;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Queries.GetHotelFeaturedDeals;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Queries.GetHotelGuestById;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Queries.GetHotelsManagement;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Queries.GetPublicHotels;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Queries.SearchHotels;
using TravelAndAccommodationBookingPlatform.Application.Features.RoomClasses.DTOs;
using TravelAndAccommodationBookingPlatform.Application.Features.RoomClasses.Queries.GetRoomClassGuest;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Hotels;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Images;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.RoomClasses;

namespace TravelAndAccommodationBookingPlatform.WebAPI.Controllers
{
    [ApiController]
    [Route("api/hotels")]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin")]
    public class HotelsController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public HotelsController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<HotelManagementResponseDto>>> GetHotelsManagement([FromQuery] GetHotelsRequestDto getHotelsRequestDto)
        {
            var query = _mapper.Map<GetHotelManagementQuery>(getHotelsRequestDto);
            var result = await _mediator.Send(query);
            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(result.PaginationMetadata);
            return Ok(result.Items);
        }


        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HotelGuestResponseDto>> GetHotelGuest(Guid id)
        {
            var query = new GetHotelGuestByIdQuery { HotelId = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("featured-deals")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<HotelFeaturedDealResponseDto>>> GetFeaturedDeals([FromQuery] GetHotelFeaturedDealsRequestDto getHotelFeaturedDealsRequestDto)
        {
            var query = _mapper.Map<GetHotelFeaturedDealsQuery>(getHotelFeaturedDealsRequestDto);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("search")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<HotelSearchResultResponseDto>>> SearchHotels([FromQuery] HotelSearchRequestDto hotelSearchRequestDto)
        {
            var query = _mapper.Map<SearchHotelsQuery>(hotelSearchRequestDto);
            var result = await _mediator.Send(query);
            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(result.PaginationMetadata);
            return Ok(result.Items);
        }

        [HttpGet("{id:guid}/room-classes")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<RoomClassGuestResponseDto>>> GetRoomClassesGuests(Guid id, [FromQuery] GetRoomClassesGuestRequestDto getRoomClassesGuestRequestDto)
        {
            var query = new GetRoomClassGuestQuery { HotelId = id };
            _mapper.Map(getRoomClassesGuestRequestDto, query);
            var result = await _mediator.Send(query);
            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(result.PaginationMetadata);
            return Ok(result.Items);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateHotel(HotelCreationRequestDto hotelCreationRequestDto)
        {
            var command = _mapper.Map<CreateHotelCommand>(hotelCreationRequestDto);
            var result = await _mediator.Send(command);
            return Created();
        }

        [HttpPost("{id:guid}/gallery")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddGalleryToHotel(Guid id, [FromForm] ImageCreationRequestDto imageCreationRequestDto)
        {
            var command = new AddGalleryToHotelCommand { HotelId = id };
            _mapper.Map(imageCreationRequestDto, command);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id:guid}/thumbnail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddHotelThumbnail(Guid id, [FromForm] ImageCreationRequestDto imageCreationRequestDto)
        {
            var command = new AddHotelThumbnailCommand { HotelId = id };
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
        public async Task<IActionResult> UpdateHotel(Guid id, HotelUpdateRequestDto hotelUpdateRequestDto)
        {
            var command = new UpdateHotelCommand { HotelId = id };
            _mapper.Map(hotelUpdateRequestDto, command);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteHotel(Guid id)
        {
            var command = new DeleteHotelCommand { HotelId = id };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("public")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<HotelSearchResultResponseDto>>> GetPublicHotels(
            [FromQuery] GetHotelsPublicRequestDto requestDto)
        {
            var query = _mapper.Map<GetPublicHotelsQuery>(requestDto);
            var result = await _mediator.Send(query);

            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(result.PaginationMetadata);
            return Ok(result.Items);
        }
    }
}
