using FinanceApp.Application.DTOs.CreditCards;

namespace FinanceApp.Application.Abstractions.Services;

public interface ICreditCardCrudService
{
    Task<IReadOnlyCollection<CreditCardDto>> ListAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<CreditCardDto?> GetAsync(Guid userId, Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(Guid userId, UpsertCreditCardDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Guid userId, Guid id, UpsertCreditCardDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid userId, Guid id, CancellationToken cancellationToken = default);
}
