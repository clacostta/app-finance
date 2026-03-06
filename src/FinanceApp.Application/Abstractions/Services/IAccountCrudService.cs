using FinanceApp.Application.DTOs.Accounts;

namespace FinanceApp.Application.Abstractions.Services;

public interface IAccountCrudService
{
    Task<IReadOnlyCollection<AccountDto>> ListAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<AccountDto?> GetAsync(Guid userId, Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(Guid userId, UpsertAccountDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Guid userId, Guid id, UpsertAccountDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid userId, Guid id, CancellationToken cancellationToken = default);
}
