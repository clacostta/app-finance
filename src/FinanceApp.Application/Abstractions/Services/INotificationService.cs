using FinanceApp.Application.DTOs.Notifications;

namespace FinanceApp.Application.Abstractions.Services;

public interface INotificationService
{
    Task<IReadOnlyCollection<NotificationDto>> ListAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<int> GenerateBudgetAlertsAsync(Guid userId, DateTime referenceDate, CancellationToken cancellationToken = default);
    Task<bool> MarkAsReadAsync(Guid userId, Guid id, CancellationToken cancellationToken = default);
}
