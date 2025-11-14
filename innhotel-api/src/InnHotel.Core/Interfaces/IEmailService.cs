namespace InnHotel.Core.Interfaces;

/// <summary>
/// Service for sending emails
/// </summary>
public interface IEmailService
{
    Task SendWelcomeEmailAsync(string email, string firstName, CancellationToken cancellationToken = default);
    Task SendThankYouEmailAsync(string email, string guestName, decimal totalAmount, CancellationToken cancellationToken = default);
    Task SendReservationConfirmationAsync(string email, string guestName, int reservationId, CancellationToken cancellationToken = default);
    Task SendCancellationConfirmationAsync(string email, string guestName, int reservationId, decimal refundAmount, CancellationToken cancellationToken = default);
    Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
}