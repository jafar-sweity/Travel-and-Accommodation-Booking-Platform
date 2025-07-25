﻿namespace TravelAndAccommodationBookingPlatform.Core.Interfaces.Auth
{
    public interface IPasswordHashService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
    }
}
