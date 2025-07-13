using AutoFixture;
using FluentAssertions;
using TravelAndAccommodationBookingPlatform.Core.Entities;

namespace TravelAndAccommodationBookingPlatform.Core.Tests.Entities;

public class CityTests
{
    private readonly Fixture _fixture;

    public CityTests()
    {
        _fixture = new Fixture();

        _fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _fixture.Register(() =>
        {
            var date = _fixture.Create<DateTime>();
            return DateOnly.FromDateTime(date);
        });
    }


    [Fact]
    public void City_Should_Inherit_From_EntityBase()
    {
        var city = new City();
        city.Should().BeAssignableTo<EntityBase>();
    }


    [Fact]
    public void City_Should_Initialize_Hotels_Collection()
    {
        var city = new City();
        city.Hotels.Should().NotBeNull();
        city.Hotels.Should().BeEmpty();
    }

    [Fact]
    public void City_Should_Set_And_Get_Properties_Correctly()
    {
        // Arrange
        var name = _fixture.Create<string>();
        var country = _fixture.Create<string>();
        var postOffice = _fixture.Create<string>();
        var region = _fixture.Create<string>();
        var createdAt = _fixture.Create<DateTime>();
        var updatedAt = _fixture.Create<DateTime?>();
        var image = _fixture.Create<Image>();

        // Act
        var city = new City
        {
            Name = name,
            Country = country,
            PostOffice = postOffice,
            Region = region,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            SmallPreview = image
        };

        // Assert
        city.Name.Should().Be(name);
        city.Country.Should().Be(country);
        city.PostOffice.Should().Be(postOffice);
        city.Region.Should().Be(region);
        city.CreatedAt.Should().Be(createdAt);
        city.UpdatedAt.Should().Be(updatedAt);
        city.SmallPreview.Should().Be(image);
    }

    [Fact]
    public void City_Should_Allow_Adding_Hotels()
    {
        // Arrange
        var city = new City();
        var hotel1 = _fixture.Create<Hotel>();
        var hotel2 = _fixture.Create<Hotel>();

        // Act
        city.Hotels.Add(hotel1);
        city.Hotels.Add(hotel2);

        // Assert
        city.Hotels.Should().HaveCount(2);
        city.Hotels.Should().Contain(hotel1);
        city.Hotels.Should().Contain(hotel2);
    }

    [Fact]
    public void City_Should_Allow_Null_SmallPreview()
    {
        var city = new City
        {
            SmallPreview = null
        };

        city.SmallPreview.Should().BeNull();
    }

    [Fact]
    public void City_Should_Allow_Null_UpdatedAt()
    {
        var city = new City
        {
            UpdatedAt = null
        };

        city.UpdatedAt.Should().BeNull();
    }
}
