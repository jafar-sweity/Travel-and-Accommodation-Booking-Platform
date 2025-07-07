using Amazon;
using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TravelAndAccommodationBookingPlatform.Infrastructure.Data;
using TravelAndAccommodationBookingPlatform.Infrastructure.Services;

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
    var awsConfig = new AmazonS3Config { RegionEndpoint = RegionEndpoint.GetBySystemName(config.Region) };
    return new AmazonS3Client(config.AccessKey, config.SecretKey, awsConfig);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
