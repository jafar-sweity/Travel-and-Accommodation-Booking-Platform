using AutoMapper;
using MediatR;
using System.Linq.Expressions;
using TravelAndAccommodationBookingPlatform.Application.Features.Owners.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Models;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Common;

namespace TravelAndAccommodationBookingPlatform.Application.Features.Owners.Queries.GetOwners
{
    public class GetOwnersQueryHandler : IRequestHandler<GetOwnersQuery, PaginatedResult<OwnerResponseDto>>
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;

        public GetOwnersQueryHandler(
            IOwnerRepository ownerRepository,
            IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<OwnerResponseDto>> Handle(GetOwnersQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Owner, bool>> filterExpression = string.IsNullOrEmpty(request.Search)
               ? _ => true : f => f.FirstName.Contains(request.Search) || f.LastName.Contains(request.Search);

            var query = new PaginatedQuery<Owner>(filterExpression, request.SortColumn, request.PageNumber, request.PageSize, request.SortDirection);
            var owner = await _ownerRepository.GetOwnersAsync(query);

            return _mapper.Map<PaginatedResult<OwnerResponseDto>>(owner);
        }
    }

}