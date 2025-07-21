namespace TravelAndAccommodationBookingPlatform.Core.Models.Email
{
    public record EmailRequest(
        IEnumerable<string> ToEmails,
        string SubjectLine,
        string MessageBody,
        IEnumerable<FileAttachment> FileAttachment
    );
}
