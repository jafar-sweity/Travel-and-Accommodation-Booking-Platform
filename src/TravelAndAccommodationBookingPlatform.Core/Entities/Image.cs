namespace TravelAndAccommodationBookingPlatform.Core.Entities
{
    public class Image : EntityBase
    {
        public Guid EntityId { get; set; }
        public string Url { get; set; } = string.Empty;
    }
}
