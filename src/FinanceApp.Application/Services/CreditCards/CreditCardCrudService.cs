using FinanceApp.Application.Abstractions;
using FinanceApp.Application.Abstractions.Services;
using FinanceApp.Application.DTOs.CreditCards;
using FinanceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Application.Services.CreditCards;

public class CreditCardCrudService : ICreditCardCrudService
{
    private readonly IAppDbContext _context;

    public CreditCardCrudService(IAppDbContext context) => _context = context;

    public async Task<IReadOnlyCollection<CreditCardDto>> ListAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.CreditCards
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Include(x => x.FinancialInstitution)
            .OrderBy(x => x.Name)
            .Select(x => new CreditCardDto(x.Id, x.Name, x.LimitAmount, x.ClosingDay, x.DueDay, x.IsActive, x.FinancialInstitutionId, x.FinancialInstitution.Name))
            .ToListAsync(cancellationToken);
    }

    public async Task<CreditCardDto?> GetAsync(Guid userId, Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.CreditCards
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.Id == id)
            .Include(x => x.FinancialInstitution)
            .Select(x => new CreditCardDto(x.Id, x.Name, x.LimitAmount, x.ClosingDay, x.DueDay, x.IsActive, x.FinancialInstitutionId, x.FinancialInstitution.Name))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Guid> CreateAsync(Guid userId, UpsertCreditCardDto dto, CancellationToken cancellationToken = default)
    {
        var card = new CreditCard(userId, dto.FinancialInstitutionId, dto.Name, dto.LimitAmount, dto.ClosingDay, dto.DueDay);
        if (!dto.IsActive) card.Update(dto.Name, dto.LimitAmount, dto.ClosingDay, dto.DueDay, false, dto.FinancialInstitutionId);

        _context.CreditCards.Add(card);
        await _context.SaveChangesAsync(cancellationToken);
        return card.Id;
    }

    public async Task<bool> UpdateAsync(Guid userId, Guid id, UpsertCreditCardDto dto, CancellationToken cancellationToken = default)
    {
        var card = await _context.CreditCards.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id, cancellationToken);
        if (card is null) return false;

        card.Update(dto.Name, dto.LimitAmount, dto.ClosingDay, dto.DueDay, dto.IsActive, dto.FinancialInstitutionId);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid userId, Guid id, CancellationToken cancellationToken = default)
    {
        var card = await _context.CreditCards.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id, cancellationToken);
        if (card is null) return false;

        _context.CreditCards.Remove(card);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
