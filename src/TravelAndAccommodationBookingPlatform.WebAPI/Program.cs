using Amazon;
using Amazon.S3;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using QuestPDF.Infrastructure;
using Serilog;
using System.Threading.RateLimiting;
using TravelAndAccommodationBookingPlatform.Application.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Infrastructure.DependencyInjection;
using TravelAndAccommodationBookingPlatform.Infrastructure.Services;
using TravelAndAccommodationBookingPlatform.WebAPI.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

QuestPDF.Settings.License = LicenseType.Community;

// AWS S3 Configuration
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

// Add Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("FixedWindowPolicy", configure =>
    {
        configure.PermitLimit = 100;
        configure.Window = TimeSpan.FromMinutes(1);
        configure.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        configure.QueueLimit = 10;
    });
});

// Add services in correct order
builder.Services.AddWebApi().AddApplication().AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TravelAndAccommodationBookingPlatform v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers().RequireRateLimiting("FixedWindowPolicy");

app.Run();