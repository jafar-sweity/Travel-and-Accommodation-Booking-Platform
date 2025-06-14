namespace TravelAndAccommodationBookingPlatform.Core.Entities
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
