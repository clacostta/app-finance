using FinanceApp.Application.Services.Notifications;
using FinanceApp.Domain.Entities;
using FinanceApp.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Tests.Application;

public class NotificationMarkReadTests
{
    [Fact]
    public async Task MarkAsReadAsync_ShouldSetIsRead()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var db = new AppDbContext(options);
        var userId = Guid.NewGuid();
        var notification = new Notification(userId, "Teste", "Mensagem");
        db.Notifications.Add(notification);
        await db.SaveChangesAsync();

        var service = new NotificationService(db);
        var result = await service.MarkAsReadAsync(userId, notification.Id);

        result.Should().BeTrue();
        (await db.Notifications.FirstAsync()).IsRead.Should().BeTrue();
    }
}
