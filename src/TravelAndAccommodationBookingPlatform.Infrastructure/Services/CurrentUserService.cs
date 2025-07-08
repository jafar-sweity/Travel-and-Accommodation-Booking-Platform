using Microsoft.AspNetCore.Http;
using TravelAndAccommodationBookingPlatform.Core.Constants.DomainMessages;
using TravelAndAccommodationBookingPlatform.Core.Constants.Exceptions;
using TravelAndAccommodationBookingPlatform.Core.Services.Authentication;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserEmail()
        {
            var emailClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("email")?.Value;

            if (string.IsNullOrEmpty(emailClaim))
                throw new UnauthorizedException(UserMessages.UserNotAuthenticated);

            return emailClaim;
        }

        public Guid GetUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedException(UserMessages.UserNotAuthenticated);

            return userId;
        }

        public string GetUserRole()
        {
            var roleClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("role")?.Value;

            if (string.IsNullOrEmpty(roleClaim))
                throw new UnauthorizedException(UserMessages.UserNotAuthenticated);

            return roleClaim;
        }
    }
}
