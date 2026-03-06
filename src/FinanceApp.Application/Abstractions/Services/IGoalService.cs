using FinanceApp.Application.DTOs.Goals;

namespace FinanceApp.Application.Abstractions.Services;

public interface IGoalService
{
    Task<IReadOnlyCollection<GoalDto>> ListAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<GoalDto?> GetAsync(Guid userId, Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(Guid userId, UpsertGoalDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Guid userId, Guid id, UpsertGoalDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid userId, Guid id, CancellationToken cancellationToken = default);
}
