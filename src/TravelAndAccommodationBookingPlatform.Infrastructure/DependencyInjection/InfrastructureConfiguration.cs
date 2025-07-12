using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Auth;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Services.Authentication;
using TravelAndAccommodationBookingPlatform.Core.Services.Authentication;
using TravelAndAccommodationBookingPlatform.Infrastructure.Auth;
using TravelAndAccommodationBookingPlatform.Infrastructure.Data;
using TravelAndAccommodationBookingPlatform.Infrastructure.Interfaces.Services;
using TravelAndAccommodationBookingPlatform.Infrastructure.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Services;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.DependencyInjection
{
    public static class InfrastructureConfiguration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext(config)
                    .AddAuthServices(config)
                    .AddRepositories()
                    .AddPasswordHashing()
                    .AddServices();

            return services;
        }

        private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("SqlServer")));

            return services;
        }



        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            services.AddScoped<IHotelRepository, HotelRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IOwnerRepository, OwnerRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IRoomClassRepository, RoomClassRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }

        private static IServiceCollection AddPasswordHashing(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHashService, PasswordHashService>();
            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IImageService, ImageService>();
            return services;
        }
    }
}
