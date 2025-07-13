using AutoFixture;
using FluentAssertions;
using TravelAndAccommodationBookingPlatform.Core.Entities;

namespace TravelAndAccommodationBookingPlatform.Core.Tests.Entities
{
    public class UserTests
    {
        private readonly Fixture _fixture;

        public UserTests()
        {
            _fixture = new Fixture();

            // Remove recursion throwing and add omit recursion
            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Customize DateTime creation to always produce valid dates within reasonable range
            _fixture.Customize<DateTime>(c => c.FromFactory(() =>
            {
                // You can adjust this range to whatever fits your domain
                var year = _fixture.Create<int>() % 100 + 1950; // 1950-2049
                var month = _fixture.Create<int>() % 12 + 1;
                var day = _fixture.Create<int>() % 28 + 1; // safe day
                return new DateTime(year, month, day);
            }));
        }

        [Fact]
        public void User_Should_Allow_Setting_Properties()
        {
            var user = new User
            {
                FirstName = "Jane",
                LastName = "Doe",
                Username = "janedoe",
                HashedPassword = "hashedpassword123",
                Email = "jane@example.com",
                DateOfBirth = new DateTime(1990, 5, 20),
                PhoneNumber = "+1234567890"
            };

            user.FirstName.Should().Be("Jane");
            user.LastName.Should().Be("Doe");
            user.Username.Should().Be("janedoe");
            user.HashedPassword.Should().Be("hashedpassword123");
            user.Email.Should().Be("jane@example.com");
            user.DateOfBirth.Should().Be(new DateTime(1990, 5, 20));
            user.PhoneNumber.Should().Be("+1234567890");
        }

        [Fact]
        public void User_Should_Allow_Adding_Roles()
        {
            var user = new User();
            var role1 = _fixture.Create<Role>();
            var role2 = _fixture.Create<Role>();

            user.Roles.Add(role1);
            user.Roles.Add(role2);

            user.Roles.Should().HaveCount(2);
        }
    }
}
