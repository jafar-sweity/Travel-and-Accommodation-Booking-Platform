using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TravelAndAccommodationBookingPlatform.WebAPI.Controllers
{
    [ApiController]
    [Route("api/debug")]
    public class DebugController : ControllerBase
    {
        [HttpGet("test-auth")]
        [Authorize]
        public IActionResult TestAuth()
        {
            var user = HttpContext.User;
            var claims = user.Claims.Select(c => new { c.Type, c.Value }).ToList();

            return Ok(new
            {
                IsAuthenticated = user.Identity?.IsAuthenticated ?? false,
                Name = user.Identity?.Name,
                Claims = claims,
                Roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList(),
                Headers = Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString())
            });
        }

        [HttpGet("test-guest")]
        [Authorize(Roles = "Guest")]
        public IActionResult TestGuest()
        {
            return Ok(new { Message = "You are authenticated as a Guest!" });
        }
    }
}