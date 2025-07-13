using AutoMapper;
using TravelAndAccommodationBookingPlatform.Application.Commands.OwnerCommands;
using TravelAndAccommodationBookingPlatform.Application.Queries.OwnerQueries;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Owners;

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
                    opt => opt.MapFrom(src => src.OrderDirection));
        }
    }
}
