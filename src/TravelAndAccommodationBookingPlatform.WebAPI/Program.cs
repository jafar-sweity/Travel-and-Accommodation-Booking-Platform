using Amazon;
using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TravelAndAccommodationBookingPlatform.Application.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Infrastructure.Data;
using TravelAndAccommodationBookingPlatform.Infrastructure.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Infrastructure.Services;
using TravelAndAccommodationBookingPlatform.WebAPI.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.Configure<AWSS3Settings>(
    builder.Configuration.GetSection("AWSS3Settings"));

builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    var config = sp.GetRequiredService<IOptions<AWSS3Settings>>().Value;

    var credentials = new Amazon.Runtime.BasicAWSCredentials(config.AccessKey, config.SecretKey);

    var s3Config = new AmazonS3Config
    {
        RegionEndpoint = RegionEndpoint.GetBySystemName(config.Region)
    };

    return new AmazonS3Client(credentials, s3Config);
});

builder.Services.AddWebApi().AddApplication().AddInfrastructure(builder.Configuration);
var app = builder.Build();

app.UseExceptionHandler();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TravelAndAccommodationBookingPlatform v1");
    c.RoutePrefix = "swagger"; // Optional: default is "swagger"
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers().RequireRateLimiting("FixedWindowPolicy");

app.Run();
