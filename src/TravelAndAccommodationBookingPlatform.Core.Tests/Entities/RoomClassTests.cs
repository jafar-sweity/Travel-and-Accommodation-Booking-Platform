using AutoFixture;
using FluentAssertions;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Enums;

namespace TravelAndAccommodationBookingPlatform.Core.Tests.Entities
{
    public class RoomClassTests
    {
        private readonly Fixture _fixture;

        public RoomClassTests()
        {
            _fixture = new Fixture();

            // Avoid circular reference issues
            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Fix DateOnly issues (if used anywhere in deep object graph)
            _fixture.Register(() =>
            {
                var date = _fixture.Create<DateTime>();
                var safeDate = date.Year < 1 || date.Year > 9999 ? DateTime.UtcNow : date;
                return DateOnly.FromDateTime(safeDate);
            });
        }

        [Fact]
        public void RoomClass_Should_Allow_Setting_Properties()
        {
            // Arrange
            var roomClass = new RoomClass
            {
                HotelId = Guid.NewGuid(),
                Name = "Luxury Suite",
                Description = "Spacious and elegant",
                NightlyRate = 199.99m,
                TypeOfRoom = RoomType.Suite,
                MaxChildrenCapacity = 2,
                MaxAdultsCapacity = 3,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Assert
            roomClass.HotelId.Should().NotBeEmpty();
            roomClass.Name.Should().Be("Luxury Suite");
            roomClass.Description.Should().Be("Spacious and elegant");
            roomClass.NightlyRate.Should().Be(199.99m);
            roomClass.TypeOfRoom.Should().Be(RoomType.Suite);
            roomClass.MaxChildrenCapacity.Should().Be(2);
            roomClass.MaxAdultsCapacity.Should().Be(3);
        }

        [Fact]
        public void RoomClass_Should_Allow_Adding_Rooms()
        {
            var roomClass = new RoomClass();
            var room1 = _fixture.Create<Room>();
            var room2 = _fixture.Create<Room>();

            roomClass.Rooms.Add(room1);
            roomClass.Rooms.Add(room2);

            roomClass.Rooms.Should().HaveCount(2);
        }

        [Fact]
        public void RoomClass_Should_Allow_Adding_Gallery_Images()
        {
            var roomClass = new RoomClass();
            var image1 = _fixture.Create<Image>();
            var image2 = _fixture.Create<Image>();

            roomClass.Gallery.Add(image1);
            roomClass.Gallery.Add(image2);

            roomClass.Gallery.Should().HaveCount(2);
        }

        [Fact]
        public void RoomClass_Should_Allow_Adding_Discounts()
        {
            var roomClass = new RoomClass();
            var discount1 = _fixture.Create<Discount>();
            var discount2 = _fixture.Create<Discount>();

            roomClass.Discounts.Add(discount1);
            roomClass.Discounts.Add(discount2);

            roomClass.Discounts.Should().HaveCount(2);
        }

        [Fact]
        public void RoomClass_Should_Handle_Hotel_Navigation_Property()
        {
            var hotel = _fixture.Create<Hotel>();
            var roomClass = new RoomClass
            {
                Hotel = hotel
            };

            roomClass.Hotel.Should().Be(hotel);
        }
    }
}
