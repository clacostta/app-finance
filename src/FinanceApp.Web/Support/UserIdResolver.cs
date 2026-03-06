using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FinanceApp.Web.Support;

public static class UserIdResolver
{
    public static Guid Resolve(ClaimsPrincipal user)
    {
        var sub = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.Identity?.Name ?? "anonymous";
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(sub));
        return new Guid(hash.Take(16).ToArray());
    }
}
