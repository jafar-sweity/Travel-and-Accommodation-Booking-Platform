using AutoMapper;
using TravelAndAccommodationBookingPlatform.Application.Features.Cities.Commands.AddCitySmallPreviewImage;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Commands.AddGalleryToHotel;
using TravelAndAccommodationBookingPlatform.Application.Features.Hotels.Commands.AddHotelThumbnail;
using TravelAndAccommodationBookingPlatform.Application.Features.RoomClasses.Commands.AddGalleryToRoomClass;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Images;

namespace TravelAndAccommodationBookingPlatform.Application.Profiles
{
    public class ImageProfile : Profile
    {
        public ImageProfile()
        {
            CreateMap<ImageCreationRequestDto, AddCitySmallPreviewImageCommand>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image));

            CreateMap<ImageCreationRequestDto, AddHotelThumbnailCommand>();
            CreateMap<ImageCreationRequestDto, AddGalleryToHotelCommand>();
            CreateMap<ImageCreationRequestDto, AddGalleryToRoomClassCommand>();
            CreateMap<ImageCreationRequestDto, AddHotelThumbnailCommand>();
        }
    }
}
