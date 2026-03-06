namespace FinanceApp.Application.Abstractions;

public interface ICurrentUserService
{
    string? IdentityUserId { get; }
}
