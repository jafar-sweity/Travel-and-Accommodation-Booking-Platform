using AutoMapper;
using TravelAndAccommodationBookingPlatform.Application.Features.Owners.Commands.CreateOwner;
using TravelAndAccommodationBookingPlatform.Application.Features.Owners.Commands.UpdateOwner;
using TravelAndAccommodationBookingPlatform.Application.Features.Owners.DTOs;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Models.DTOs.Common;

namespace TravelAndAccommodationBookingPlatform.Application.Profiles
{
    class OwnerProfile : Profile
    {
        public OwnerProfile()
        {
            CreateMap<Owner, OwnerResponseDto>();
            CreateMap<CreateOwnerCommand, Owner>();
            CreateMap(typeof(PaginatedResult<>), typeof(PaginatedResult<>));
            CreateMap<UpdateOwnerCommand, Owner>();
        }
    }
}
