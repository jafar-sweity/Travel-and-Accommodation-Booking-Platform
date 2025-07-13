using AutoFixture;
using FluentAssertions;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Enums;

namespace TravelAndAccommodationBookingPlatform.Core.Tests.Entities
{
    public class HotelTests
    {
        private readonly Fixture _fixture;

        public HotelTests()
        {
            _fixture = new Fixture();

            // Prevent recursion loops (Hotel -> RoomClass -> Room -> Invoice -> Booking -> Hotel)
            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Safe DateOnly generator
            _fixture.Register(() =>
            {
                var date = _fixture.Create<DateTime>();
                var safeDate = date.Year < 1 || date.Year > 9999 ? DateTime.UtcNow : date;
                return DateOnly.FromDateTime(safeDate);
            });
        }


        [Fact]
        public void Hotel_Should_Allow_Setting_Properties()
        {
            var hotel = new Hotel
            {
                Name = "Test Hotel",
                CityId = Guid.NewGuid(),
                OwnerId = Guid.NewGuid(),
                ReviewsRating = 4.8,
                StarRating = 5,
                Website = "https://example.com",
                BriefDescription = "Short desc",
                FullDescription = "Longer description",
                Geolocation = "32.0853, 34.7818",
                Status = HotelStatus.Open,
                PhoneNumber = "123456789",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            hotel.Name.Should().Be("Test Hotel");
            hotel.CityId.Should().NotBeEmpty();
            hotel.OwnerId.Should().NotBeEmpty();
            hotel.ReviewsRating.Should().BeGreaterThan(0);
            hotel.StarRating.Should().Be(5);
            hotel.Status.Should().Be(HotelStatus.Open);
            hotel.PhoneNumber.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void Hotel_Should_Allow_Adding_RoomClasses()
        {
            var hotel = new Hotel();
            var roomClass1 = _fixture.Create<RoomClass>();
            var roomClass2 = _fixture.Create<RoomClass>();

            hotel.RoomClasses.Add(roomClass1);
            hotel.RoomClasses.Add(roomClass2);

            hotel.RoomClasses.Should().HaveCount(2);
        }

        [Fact]
        public void Hotel_Should_Allow_Adding_Bookings()
        {
            var hotel = new Hotel();
            var booking1 = _fixture.Create<Booking>();
            var booking2 = _fixture.Create<Booking>();

            hotel.Bookings.Add(booking1);
            hotel.Bookings.Add(booking2);

            hotel.Bookings.Should().HaveCount(2);
        }

        [Fact]
        public void Hotel_Should_Allow_Adding_Reviews()
        {
            var hotel = new Hotel();
            var review1 = _fixture.Create<Review>();
            var review2 = _fixture.Create<Review>();

            hotel.Reviews.Add(review1);
            hotel.Reviews.Add(review2);

            hotel.Reviews.Should().HaveCount(2);
        }

        [Fact]
        public void Hotel_Should_Allow_Adding_Images()
        {
            var hotel = new Hotel();
            var img1 = _fixture.Create<Image>();
            var img2 = _fixture.Create<Image>();

            hotel.FullView.Add(img1);
            hotel.FullView.Add(img2);

            hotel.FullView.Should().HaveCount(2);
        }

        [Fact]
        public void Hotel_Should_Handle_Navigation_Properties()
        {
            var city = _fixture.Create<City>();
            var owner = _fixture.Create<Owner>();
            var hotel = new Hotel { City = city, Owner = owner };

            hotel.City.Should().Be(city);
            hotel.Owner.Should().Be(owner);
        }
    }
}
