namespace FinanceApp.Application.DTOs.CreditCards;

public record CreditCardDto(Guid Id, string Name, decimal LimitAmount, int ClosingDay, int DueDay, bool IsActive, Guid FinancialInstitutionId, string? FinancialInstitutionName);
