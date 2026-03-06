using FinanceApp.Application.Abstractions;
using FinanceApp.Domain.Entities;
using FinanceApp.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<AppIdentityUser>, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<UserPreference> UserPreferences => Set<UserPreference>();
    public DbSet<FinancialInstitution> FinancialInstitutions => Set<FinancialInstitution>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<CreditCard> CreditCards => Set<CreditCard>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<TransactionCategory> TransactionCategories => Set<TransactionCategory>();
    public DbSet<TransactionSubcategory> TransactionSubcategories => Set<TransactionSubcategory>();
    public DbSet<ImportBatch> ImportBatches => Set<ImportBatch>();
    public DbSet<ImportedFile> ImportedFiles => Set<ImportedFile>();
    public DbSet<CategorizationRule> CategorizationRules => Set<CategorizationRule>();
    public DbSet<Budget> Budgets => Set<Budget>();
    public DbSet<Goal> Goals => Set<Goal>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<SubscriptionDetection> SubscriptionDetections => Set<SubscriptionDetection>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasIndex(x => x.IdentityUserId).IsUnique();
            entity.Property(x => x.FullName).HasMaxLength(150).IsRequired();
            entity.Property(x => x.CurrencyCode).HasMaxLength(3).IsRequired();
        });

        builder.Entity<Transaction>(entity =>
        {
            entity.HasIndex(x => new { x.UserId, x.TransactionDate });
            entity.HasIndex(x => new { x.UserId, x.ExternalId });
            entity.Property(x => x.Amount).HasPrecision(18, 2);
            entity.Property(x => x.Description).HasMaxLength(250).IsRequired();
        });


        builder.Entity<ImportBatch>(entity =>
        {
            entity.HasIndex(x => new { x.UserId, x.FileHash }).IsUnique();
            entity.Property(x => x.FileName).HasMaxLength(255).IsRequired();
            entity.Property(x => x.FileHash).HasMaxLength(128).IsRequired();
        });

        builder.Entity<Account>().Property(x => x.InitialBalance).HasPrecision(18, 2);
        builder.Entity<CreditCard>().Property(x => x.LimitAmount).HasPrecision(18, 2);
        builder.Entity<Budget>().Property(x => x.PlannedAmount).HasPrecision(18, 2);
        builder.Entity<Goal>().Property(x => x.TargetAmount).HasPrecision(18, 2);
        builder.Entity<Goal>().Property(x => x.CurrentAmount).HasPrecision(18, 2);
    }
}
