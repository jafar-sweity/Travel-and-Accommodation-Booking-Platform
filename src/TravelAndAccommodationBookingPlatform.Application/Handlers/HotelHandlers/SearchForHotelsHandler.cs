using AutoMapper;
using MediatR;
using System.Linq.Expressions;
using TravelAndAccommodationBookingPlatform.Application.DTOs.HotelDtos;
using TravelAndAccommodationBookingPlatform.Application.Queries.HotelQueries;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Enums;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Models;

namespace TravelAndAccommodationBookingPlatform.Application.Handlers.HotelHandlers
{
    public class SearchForHotelsHandler : IRequestHandler<SearchHotelsQuery, PaginatedResult<HotelSearchResultResponseDto>>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;

        public SearchForHotelsHandler(IHotelRepository hotelRepository, IMapper mapper)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<HotelSearchResultResponseDto>> Handle(SearchHotelsQuery request, CancellationToken cancellationToken)
        {
            var filter = BuildSearchFilter(request);

            var query = new PaginatedQuery<Hotel>(
                filter,
                request.SortColumn,
                request.PageNumber,
                request.PageSize,
                request.OrderDirection ?? OrderDirection.Ascending
            );

            var hotels = await _hotelRepository.FindHotelsAsync(query);
            return _mapper.Map<PaginatedResult<HotelSearchResultResponseDto>>(hotels);
        }

        private static Expression<Func<Hotel, bool>> BuildSearchFilter(SearchHotelsQuery request)
        {
            Expression<Func<Hotel, bool>> filter = h => true;

            if (!string.IsNullOrWhiteSpace(request.Search))
                filter = filter.And(h =>
                    h.Name.Contains(request.Search) ||
                    h.City.Name.Contains(request.Search) ||
                    h.City.Country.Contains(request.Search)
                );

            if (request.MinStarRating.HasValue)
                filter = filter.And(h => h.StarRating >= request.MinStarRating);

            if (request.MinPrice.HasValue)
                filter = filter.And(h => h.RoomClasses.Any(rc => rc.NightlyRate >= request.MinPrice));

            if (request.MaxPrice.HasValue)
                filter = filter.And(h => h.RoomClasses.Any(rc => rc.NightlyRate <= request.MaxPrice));

            if (request.RoomTypes?.Any() == true)
                filter = filter.And(h => h.RoomClasses.Any(rc => request.RoomTypes.Contains(rc.TypeOfRoom)));

            filter = filter.And(h =>
                h.RoomClasses.Any(rc =>
                    rc.MaxAdultsCapacity >= request.MaxAdultsCapacity &&
                    rc.MaxChildrenCapacity >= request.MaxChildrenCapacity &&
                    rc.Rooms.Count(r =>
                        !r.Bookings.Any(b =>
                            b.CheckOutDate > request.CheckInDate &&
                            b.CheckInDate < request.CheckOutDate
                        )
                    ) >= request.NumberOfRooms
                )
            );

            return filter;
        }
    }

    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var param = Expression.Parameter(typeof(T));
            var body = Expression.AndAlso(
                Expression.Invoke(expr1, param),
                Expression.Invoke(expr2, param)
            );
            return Expression.Lambda<Func<T, bool>>(body, param);
        }
    }
}
