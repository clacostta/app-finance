using Microsoft.AspNetCore.Identity;

namespace FinanceApp.Infrastructure.Identity;

public class AppIdentityUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}
