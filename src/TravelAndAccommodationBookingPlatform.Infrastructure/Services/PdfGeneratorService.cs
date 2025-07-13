using QuestPDF.Fluent;
using QuestPDF.Helpers;
using TravelAndAccommodationBookingPlatform.Core.Entities;
using TravelAndAccommodationBookingPlatform.Core.Interfaces.Services;

namespace TravelAndAccommodationBookingPlatform.Infrastructure.Services
{
    public class PdfGeneratorService : IPdfGeneratorService
    {
        public Task<byte[]> GenerateInvoiceAsync(Booking booking)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(14));

                    page.Content()
                        .Column(column =>
                        {
                            column.Item().Text($"Invoice for Booking ID: {booking.Id}")
                                .FontSize(20).Bold().AlignCenter();

                            column.Item().PaddingVertical(10).LineHorizontal(1);

                            column.Item().Text($"Guest: {booking.Guest?.FirstName ?? "N/A"}");
                            column.Item().Text($"Email: {booking.Guest?.Email ?? "N/A"}");
                            column.Item().Text($"Phone: {booking.Guest?.PhoneNumber ?? "N/A"}");

                            column.Item().PaddingVertical(10).LineHorizontal(1);

                            column.Item().Text($"Hotel: {booking.Hotel?.Name ?? "N/A"}");
                            column.Item().Text($"Check-in: {booking.CheckInDate:dd MMM yyyy}");
                            column.Item().Text($"Check-out: {booking.CheckOutDate:dd MMM yyyy}");
                            column.Item().Text($"Total Nights: {(booking.CheckOutDate.ToDateTime(TimeOnly.MinValue) - booking.CheckInDate.ToDateTime(TimeOnly.MinValue)).Days}");
                            column.Item().Text($"Total Price: ${booking.TotalPrice:F2}");

                            column.Item().PaddingTop(30).AlignCenter().Text("Thank you for booking with us!")
                                .Italic().FontSize(12).FontColor(Colors.Grey.Darken2);
                        });
                });
            });

            byte[] pdfBytes = document.GeneratePdf();
            return Task.FromResult(pdfBytes);
        }
    }
}
