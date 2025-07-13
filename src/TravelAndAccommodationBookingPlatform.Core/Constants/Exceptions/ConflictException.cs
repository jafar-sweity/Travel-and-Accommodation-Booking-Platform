namespace TravelAndAccommodationBookingPlatform.Core.Constants.Exceptions
{
    public class ConflictException : CustomException
    {
        public ConflictException(string message) : base(message) { }
        public override string Title => "Conflict";
        public override string Details => "A conflict occurred during the operation.";
    }
}
