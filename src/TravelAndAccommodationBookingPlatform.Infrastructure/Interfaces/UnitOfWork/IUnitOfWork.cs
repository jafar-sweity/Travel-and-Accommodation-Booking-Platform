namespace TravelAndAccommodationBookingPlatform.Infrastructure.Interfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}
