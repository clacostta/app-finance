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
    DbSet<Budget> Budgets { get; }
    DbSet<Goal> Goals { get; }
    DbSet<Notification> Notifications { get; }
    DbSet<SubscriptionDetection> SubscriptionDetections { get; }
    DbSet<ImportBatch> ImportBatches { get; }
    DbSet<ImportedFile> ImportedFiles { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
