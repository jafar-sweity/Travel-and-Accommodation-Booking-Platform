﻿using MediatR;
using TravelAndAccommodationBookingPlatform.Application.Features.Discounts.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Enums;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Common;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Discounts.Queries.GetDiscounts
{
    public class GetDiscountsQuery : IRequest<PaginatedResult<DiscountResponseDto>>
    {
        public Guid RoomClassId { get; init; }
        public OrderDirection? OrderDirection { get; init; }
        public string? SortColumn { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
    }
}
