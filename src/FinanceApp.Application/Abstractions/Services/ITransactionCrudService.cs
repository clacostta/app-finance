using FinanceApp.Application.DTOs.Transactions;

namespace FinanceApp.Application.Abstractions.Services;

public interface ITransactionCrudService
{
    Task<IReadOnlyCollection<TransactionDto>> ListAsync(Guid userId, DateTime? from, DateTime? to, string? search, CancellationToken cancellationToken = default);
    Task<TransactionDto?> GetAsync(Guid userId, Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(Guid userId, UpsertTransactionDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Guid userId, Guid id, UpsertTransactionDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid userId, Guid id, CancellationToken cancellationToken = default);
}
