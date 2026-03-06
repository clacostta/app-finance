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

        await context.SaveChangesAsync();
    }
}
