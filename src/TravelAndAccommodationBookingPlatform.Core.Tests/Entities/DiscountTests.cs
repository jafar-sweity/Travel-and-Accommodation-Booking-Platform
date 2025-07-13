using AutoFixture;
using FluentAssertions;
using TravelAndAccommodationBookingPlatform.Core.Entities;

namespace TravelAndAccommodationBookingPlatform.Core.Tests.Entities
{
    public class DiscountTests
    {
        private readonly Fixture _fixture;

        public DiscountTests()
        {
            _fixture = new Fixture();

            // Handle recursion issues
            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Register safe DateOnly generator
            _fixture.Register(() =>
            {
                var date = _fixture.Create<DateTime>();
                var safeDate = date.Year < 1 || date.Year > 9999
                    ? DateTime.UtcNow
                    : date;
                return DateOnly.FromDateTime(safeDate);
            });
        }


        [Fact]
        public void Discount_Should_Inherit_From_EntityBase()
        {
            // Arrange & Act
            var discount = new Discount();

            // Assert
            discount.Should().BeAssignableTo<EntityBase>();
        }

        [Fact]
        public void Discount_Should_Allow_Setting_And_Getting_Properties()
        {
            // Arrange
            var roomClassId = _fixture.Create<Guid>();
            var roomClass = _fixture.Create<RoomClass>();
            var percentage = _fixture.Create<decimal>();
            var description = _fixture.Create<string>();
            var startDate = _fixture.Create<DateTime>();
            var endDate = _fixture.Create<DateTime>();
            var createdAt = _fixture.Create<DateTime>();

            // Act
            var discount = new Discount
            {
                RoomClassId = roomClassId,
                RoomClass = roomClass,
                Percentage = percentage,
                Description = description,
                StartDate = startDate,
                EndDate = endDate,
                CreatedAt = createdAt
            };

            // Assert
            discount.RoomClassId.Should().Be(roomClassId);
            discount.RoomClass.Should().Be(roomClass);
            discount.Percentage.Should().Be(percentage);
            discount.Description.Should().Be(description);
            discount.StartDate.Should().Be(startDate);
            discount.EndDate.Should().Be(endDate);
            discount.CreatedAt.Should().Be(createdAt);
        }

        [Fact]
        public void Discount_Should_Allow_Null_Description()
        {
            var discount = new Discount
            {
                Description = null
            };

            discount.Description.Should().BeNull();
        }
    }
}
