using FinanceApp.Domain.Common;

namespace FinanceApp.Domain.Entities;

public class AuditLog : BaseEntity
{
    public Guid UserId { get; private set; }
    public string Action { get; private set; } = string.Empty;
    public string Resource { get; private set; } = string.Empty;
    public string? OldValues { get; private set; }
    public string? NewValues { get; private set; }
    public string IpAddress { get; private set; } = string.Empty;

    private AuditLog() { }

    public AuditLog(Guid userId, string action, string resource, string ipAddress)
    {
        UserId = userId;
        Action = action;
        Resource = resource;
        IpAddress = ipAddress;
    }
}
