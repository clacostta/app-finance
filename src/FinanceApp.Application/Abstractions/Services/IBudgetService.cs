using FinanceApp.Application.DTOs.Budgets;

namespace FinanceApp.Application.Abstractions.Services;

public interface IBudgetService
{
    Task<IReadOnlyCollection<BudgetDto>> ListAsync(Guid userId, int year, int month, CancellationToken cancellationToken = default);
    Task<BudgetDto?> GetAsync(Guid userId, Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(Guid userId, UpsertBudgetDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Guid userId, Guid id, UpsertBudgetDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid userId, Guid id, CancellationToken cancellationToken = default);
}
