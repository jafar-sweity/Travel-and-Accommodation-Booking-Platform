using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAndAccommodationBookingPlatform.Core.Entities;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasIndex(u => u.Email).IsUnique();

            builder.HasData([
                     new User
                  {
                    Id = new Guid("7E754E75-D677-4483-57BD-08DD21B65A13"),
                    Username= "admin",
                    FirstName = "Admin",
                    LastName = "Admin",
                    Email = "admin@Test.com",
                    PhoneNumber = "0569345887",
                    HashedPassword = "$2a$11$1lJdLae7aN7N9WQYScivdOhU8.1PjaZqtVnfhqQwS.RR0Kavf.27i"
                  }]);

            builder.HasMany(u => u.Roles)
                   .WithMany(r => r.Users)
                   .UsingEntity<Dictionary<string, object>>("UserRole", j => j.HasOne<Role>().WithMany()
                   .HasForeignKey("RoleId").OnDelete(DeleteBehavior.Cascade), j => j.HasOne<User>().WithMany()
                   .HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade))
                   .HasData([new Dictionary<string, object> { ["UserId"] = new Guid("7E754E75-D677-4483-57BD-08DD21B65A13"), ["RoleId"] = new Guid("6979DA61-A3BA-42DE-AB1A-08DD21B746D6") }]);
        }
    }
}
