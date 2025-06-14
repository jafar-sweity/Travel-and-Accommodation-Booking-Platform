namespace TravelAndAccommodationBookingPlatform.Core.Entities
{
    public class Role : EntityBase
    {
        public string name { get; set; }
        public string description { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();

    }
}
