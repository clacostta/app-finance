using FinanceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Application.Abstractions;

public interface IAppDbContext
{
    DbSet<User> Users { get; }
    DbSet<FinancialInstitution> FinancialInstitutions { get; }
    DbSet<Account> Accounts { get; }
    DbSet<CreditCard> CreditCards { get; }
    DbSet<Transaction> Transactions { get; }
    DbSet<TransactionCategory> TransactionCategories { get; }
    DbSet<ImportBatch> ImportBatches { get; }
    DbSet<ImportedFile> ImportedFiles { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
