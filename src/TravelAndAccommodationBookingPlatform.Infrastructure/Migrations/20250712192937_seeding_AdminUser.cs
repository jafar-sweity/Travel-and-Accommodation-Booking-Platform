using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seeding_AdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DateOfBirth", "Email", "FirstName", "HashedPassword", "LastName", "PhoneNumber", "Username" },
                values: new object[] { new Guid("7e754e75-d677-4483-57bd-08dd21b65a13"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@Test.com", "Admin", "$2a$11$1lJdLae7aN7N9WQYScivdOhU8.1PjaZqtVnfhqQwS.RR0Kavf.27i", "Admin", "0569345887", "admin" });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("6979da61-a3ba-42de-ab1a-08dd21b746d6"), new Guid("7e754e75-d677-4483-57bd-08dd21b65a13") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("6979da61-a3ba-42de-ab1a-08dd21b746d6"), new Guid("7e754e75-d677-4483-57bd-08dd21b65a13") });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("7e754e75-d677-4483-57bd-08dd21b65a13"));
        }
    }
}
