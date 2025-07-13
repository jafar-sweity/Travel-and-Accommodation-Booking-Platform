using FluentAssertions;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Services.Authentication;

namespace TravelAndAccommodationBookingPlatform.Core.Tests.Services.Authentication
{
    public class PasswordHashServiceTests
    {
        private readonly PasswordHashService _passwordHashService;

        public PasswordHashServiceTests()
        {
            _passwordHashService = new PasswordHashService();
        }

        [Fact]
        public void HashPassword_Should_Return_NonEmpty_Hash()
        {
            // Arrange
            var password = "MySecureP@ssw0rd";

            // Act
            var hashed = _passwordHashService.HashPassword(password);

            // Assert
            hashed.Should().NotBeNullOrEmpty();
            hashed.Should().NotBe(password);
        }

        [Fact]
        public void VerifyPassword_Should_Return_True_For_Correct_Password()
        {
            // Arrange
            var password = "MySecureP@ssw0rd";
            var hashed = _passwordHashService.HashPassword(password);

            // Act
            var result = _passwordHashService.VerifyPassword(password, hashed);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void VerifyPassword_Should_Return_False_For_Incorrect_Password()
        {
            // Arrange
            var password = "MySecureP@ssw0rd";
            var wrongPassword = "WrongPassword123";
            var hashed = _passwordHashService.HashPassword(password);

            // Act
            var result = _passwordHashService.VerifyPassword(wrongPassword, hashed);

            // Assert
            result.Should().BeFalse();
        }
    }
}
