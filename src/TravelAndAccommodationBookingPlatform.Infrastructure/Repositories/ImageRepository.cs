using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Repositories;
using TravelAndAccommodationBookingPlatform.Infrastructure.Data;
using TravelAndAccommodationBookingPlatform.Infrastructure.Interfaces.Services;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Repositories
{
    class ImageRepository : IImageRepository
    {
        private readonly AppDbContext _context;
        private readonly IImageService _imageService;

        public ImageRepository(AppDbContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task RemoveImageAsync(Guid entityId)
        {
            var images = await _context.Images.Where(i => i.EntityId == entityId).ToListAsync();

            foreach (var image in images)
            {
                await _imageService.RemoveAsync(image);
                _context.Images.Remove(image);
            }
        }

        public async Task<Image> UploadImageAsync(IFormFile image, Guid entityId)
        {
            var storedImage = await _imageService.StoreAsync(image);

            var imageEntity = new Image
            {
                EntityId = entityId,
                Url = storedImage.Url
            };

            var createdImage = await _context.Images.AddAsync(imageEntity);
            return createdImage.Entity;
        }
    }
}
