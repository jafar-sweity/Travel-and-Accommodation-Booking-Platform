using AutoMapper;
using TravelAndAccommodationBookingPlatform.Application.Commands.CityCommands;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Images;

namespace TravelAndAccommodationBookingPlatform.Application.Profiles
{
    public class ImageProfile : Profile
    {
        public ImageProfile()
        {
            CreateMap<ImageCreationRequestDto, AddCitySmallPreviewImageCommand>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image));
        }
    }
}
