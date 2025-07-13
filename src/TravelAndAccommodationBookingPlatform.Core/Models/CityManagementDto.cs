namespace TravelAndAccommodationBookingPlatform.Core.Models
{
    /// <summary>
    /// Represents the data model used to display city information in the admin panel.
    /// This view includes summarized info about the city along with the total number of hotels.
    /// </summary>
    public class CityManagementDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int TotalHotels { get; set; }
        public string Country { get; set; }
        public string PostOffice { get; set; }
        public string Region { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
