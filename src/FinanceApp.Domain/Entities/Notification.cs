using FinanceApp.Domain.Common;

namespace FinanceApp.Domain.Entities;

public class Notification : BaseEntity
{
    public Guid UserId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Message { get; private set; } = string.Empty;
    public bool IsRead { get; private set; }

    private Notification() { }

    public Notification(Guid userId, string title, string message)
    {
        UserId = userId;
        Title = title;
        Message = message;
    }
}
