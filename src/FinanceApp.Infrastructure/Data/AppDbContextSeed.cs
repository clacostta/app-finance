using FinanceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Data;

public static class AppDbContextSeed
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (!await context.FinancialInstitutions.AnyAsync())
        {
            context.FinancialInstitutions.AddRange(
                new FinancialInstitution("Banco do Brasil", "001"),
                new FinancialInstitution("Itaú", "341"),
                new FinancialInstitution("Nubank", "260"));
        }

        if (!await context.TransactionCategories.AnyAsync(x => x.IsSystemDefault))
        {
            var systemUserId = Guid.Empty;
            context.TransactionCategories.AddRange(
                new TransactionCategory(systemUserId, "Alimentação", true),
                new TransactionCategory(systemUserId, "Moradia", true),
                new TransactionCategory(systemUserId, "Transporte", true),
                new TransactionCategory(systemUserId, "Lazer", true),
                new TransactionCategory(systemUserId, "Saúde", true),   
                new TransactionCategory(systemUserId, "Educação", true),
                new TransactionCategory(systemUserId, "Salário", true));
        } 

        await context.SaveChangesAsync();
    }
}
