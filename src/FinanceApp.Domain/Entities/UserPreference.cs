using FinanceApp.Domain.Common;

namespace FinanceApp.Domain.Entities;

public class UserPreference : BaseEntity
{
    public Guid UserId { get; private set; }
    public bool EnableEmailNotifications { get; private set; } = true;
    public bool EnablePushNotifications { get; private set; } = true;
    public string Theme { get; private set; } = "light";
    public string Language { get; private set; } = "pt-BR";

    public User User { get; private set; } = default!;

    private UserPreference() { }

    public UserPreference(Guid userId)
    {
        UserId = userId;
    }
}
