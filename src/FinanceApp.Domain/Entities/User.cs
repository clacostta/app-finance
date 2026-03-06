using FinanceApp.Domain.Common;

namespace FinanceApp.Domain.Entities;

public class User : BaseEntity
{
    public string IdentityUserId { get; private set; } = string.Empty;
    public string FullName { get; private set; } = string.Empty;
    public string TimeZone { get; private set; } = "America/Sao_Paulo";
    public string CurrencyCode { get; private set; } = "BRL";

    public UserPreference? Preferences { get; private set; }
    public ICollection<Account> Accounts { get; private set; } = new List<Account>();

    private User() { }

    public User(string identityUserId, string fullName)
    {
        IdentityUserId = identityUserId;
        FullName = fullName;
    }
}
