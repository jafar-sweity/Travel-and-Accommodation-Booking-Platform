using FluentValidation;
using TravelAndAccommodationBookingPlatform.WebAPI.DTOs.Images;

namespace TravelAndAccommodationBookingPlatform.WebAPI.Validators.Images
{
    public class ImageCreationRequestValidator : AbstractValidator<ImageCreationRequestDto>
    {

        public ImageCreationRequestValidator()
        {
            RuleFor(x => x.Image)
                .NotNull().WithMessage("Image file is required.")
                .Must(image => image.Length > 0).WithMessage("Image file must not be empty.");
        }
    }
}