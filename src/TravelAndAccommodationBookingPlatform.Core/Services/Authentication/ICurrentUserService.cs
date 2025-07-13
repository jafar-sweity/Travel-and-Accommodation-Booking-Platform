namespace TravelAndAccommodationBookingPlatform.Core.Services.Authentication
{
    public interface ICurrentUserService
    {
        Guid GetUserId();
        string GetUserRole();
        string GetUserEmail();
    }
}
