using System.Security.Claims;

namespace Mistake2.Data;

public class User
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";

    // Wrong because create ClaimsPrincipal without authentication type.
    public ClaimsPrincipal ToClaimsPrincipal() => new(new ClaimsIdentity(new Claim[]
    {
        new (ClaimTypes.Name, Username),
        new (ClaimTypes.Hash, Password),
    }, "Blazor School"));

    public static User FromClaimsPrincipal(ClaimsPrincipal principal) => new()
    {
        Username = principal.FindFirstValue(ClaimTypes.Name),
        Password = principal.FindFirstValue(ClaimTypes.Hash)
    };
}