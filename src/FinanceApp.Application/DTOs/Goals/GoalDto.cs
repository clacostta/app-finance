namespace FinanceApp.Application.DTOs.Goals;

public record GoalDto(Guid Id, string Name, decimal TargetAmount, decimal CurrentAmount, DateTime? TargetDate, decimal ProgressPercent);
