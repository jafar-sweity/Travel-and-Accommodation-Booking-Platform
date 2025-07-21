namespace TravelAndAccommodationBookingPlatform.Core.Models.Email
{
    public record FileAttachment(
        string FileName,
        string MediaType,
        byte[] FileContent
    );
}
