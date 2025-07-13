using System.Text.Json;
using System.Text.Json.Serialization;

namespace TravelAndAccommodationBookingPlatform.WebAPI.DependencyInjection
{
    public static class WebApiConfiguration
    {
        public static IServiceCollection AddWebApi(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddSwaggerSetup();
            services.AddProblemDetails();

            // Add controllers with JSON configuration
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}