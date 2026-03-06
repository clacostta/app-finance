using FinanceApp.Application.Abstractions;
using FinanceApp.Application.Abstractions.Services;
using FinanceApp.Application.Services;
using FinanceApp.Application.Services.Accounts;
using FinanceApp.Application.Services.Categories;
using FinanceApp.Application.Services.CreditCards;
using FinanceApp.Application.Services.Budgets;
using FinanceApp.Application.Services.Goals;
using FinanceApp.Application.Services.Notifications;
using FinanceApp.Application.Services.Subscriptions;
using FinanceApp.Application.Services.Dashboard;
using FinanceApp.Application.Services.Insights;
using FinanceApp.Application.Services.Reports;
using FinanceApp.Application.Services.Transactions;
using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Identity;
using FinanceApp.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceApp.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var provider = configuration.GetValue<string>("DatabaseProvider") ?? "PostgreSql";
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection was not configured.");

        services.AddDbContext<AppDbContext>(options =>
        {
            if (provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
            {
                options.UseSqlServer(connectionString);
            }
            else if (provider.Equals("Sqlite", StringComparison.OrdinalIgnoreCase))
            {
                options.UseSqlite(connectionString);
            }
            else
            {
                options.UseNpgsql(connectionString);
            }
        });

        services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped<DashboardService>();
        services.AddScoped<IDashboardAnalyticsService, DashboardAnalyticsService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IInsightService, InsightService>();
        services.AddScoped<IOfxImportService, OfxImportService>();
        services.AddScoped<IOfxParser, OfxParser>();
        services.AddScoped<IAccountCrudService, AccountCrudService>();
        services.AddScoped<ICreditCardCrudService, CreditCardCrudService>();
        services.AddScoped<ICategoryCrudService, CategoryCrudService>();
        services.AddScoped<ITransactionCrudService, TransactionCrudService>();
        services.AddScoped<IBudgetService, BudgetService>();
        services.AddScoped<IGoalService, GoalService>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        services.AddScoped<INotificationService, NotificationService>();

        services.AddIdentity<AppIdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders()
            .AddSignInManager();

        return services;
    }
}
