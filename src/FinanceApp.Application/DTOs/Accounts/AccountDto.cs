namespace FinanceApp.Application.DTOs.Accounts;

public record AccountDto(Guid Id, string Name, decimal InitialBalance, bool IsActive, Guid FinancialInstitutionId, string? FinancialInstitutionName);
