using FinanceApp.Application.Abstractions;
using FinanceApp.Application.Abstractions.Services;
using FinanceApp.Application.DTOs.Accounts;
using FinanceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Application.Services.Accounts;

public class AccountCrudService : IAccountCrudService
{
    private readonly IAppDbContext _context;

    public AccountCrudService(IAppDbContext context) => _context = context;

    public async Task<IReadOnlyCollection<AccountDto>> ListAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Accounts
            .Where(x => x.UserId == userId)
            .Include(x => x.FinancialInstitution)
            .OrderBy(x => x.Name)
            .Select(x => new AccountDto(x.Id, x.Name, x.InitialBalance, x.IsActive, x.FinancialInstitutionId, x.FinancialInstitution.Name))
            .ToListAsync(cancellationToken);
    }

    public async Task<AccountDto?> GetAsync(Guid userId, Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Accounts
            .Where(x => x.UserId == userId && x.Id == id)
            .Include(x => x.FinancialInstitution)
            .Select(x => new AccountDto(x.Id, x.Name, x.InitialBalance, x.IsActive, x.FinancialInstitutionId, x.FinancialInstitution.Name))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Guid> CreateAsync(Guid userId, UpsertAccountDto dto, CancellationToken cancellationToken = default)
    {
        var account = new Account(userId, dto.FinancialInstitutionId, dto.Name, dto.InitialBalance);
        if (!dto.IsActive) account.Update(dto.Name, dto.InitialBalance, false, dto.FinancialInstitutionId);

        _context.Accounts.Add(account);
        await _context.SaveChangesAsync(cancellationToken);
        return account.Id;
    }

    public async Task<bool> UpdateAsync(Guid userId, Guid id, UpsertAccountDto dto, CancellationToken cancellationToken = default)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id, cancellationToken);
        if (account is null) return false;

        account.Update(dto.Name, dto.InitialBalance, dto.IsActive, dto.FinancialInstitutionId);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid userId, Guid id, CancellationToken cancellationToken = default)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id, cancellationToken);
        if (account is null) return false;

        _context.Accounts.Remove(account);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
