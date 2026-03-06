namespace FinanceApp.Application.DTOs.Subscriptions;

public record SubscriptionDto(Guid Id, string Merchant, decimal AverageAmount, int OccurrenceCount, decimal AnnualProjection, bool IsActive);
