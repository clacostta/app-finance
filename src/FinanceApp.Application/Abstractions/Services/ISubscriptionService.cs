using FinanceApp.Application.DTOs.Subscriptions;

namespace FinanceApp.Application.Abstractions.Services;

public interface ISubscriptionService
{
    Task<IReadOnlyCollection<SubscriptionDto>> ListAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<int> RefreshDetectionsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> DeactivateAsync(Guid userId, Guid id, CancellationToken cancellationToken = default);
}
