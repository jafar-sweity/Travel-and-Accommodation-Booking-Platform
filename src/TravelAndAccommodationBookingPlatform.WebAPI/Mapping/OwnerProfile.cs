using AutoMapper;
using TravelAndAccommodationBookingPlatform.Application.Features.Owners.Commands.CreateOwner;
using TravelAndAccommodationBookingPlatform.Application.Features.Owners.Commands.UpdateOwner;
using TravelAndAccommodationBookingPlatform.Application.Features.Owners.Queries.GetOwners;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Owners;
using TravelAndAccommodationBookingPlatform.WebAPI.Helpers;

namespace TravelAndAccommodationBookingPlatform.WebAPI.Mapping
{
    public class OwnerProfile : Profile
    {
        public OwnerProfile()
        {
            CreateMap<OwnerCreationRequestDto, CreateOwnerCommand>();
            CreateMap<OwnerUpdateRequestDto, UpdateOwnerCommand>();
            CreateMap<GetOwnersRequestDto, GetOwnersQuery>()
                .ForMember(
                    dst => dst.SortDirection,
                    opt => opt.MapFrom(src => MappingHelpers.MapOrderDirection(src.OrderDirection)));
        }
    }
}
