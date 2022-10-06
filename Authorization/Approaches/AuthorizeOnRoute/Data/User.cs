using System.Security.Claims;

namespace AuthorizeOnRoute.Data;

public class User
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public List<string> Roles { get; set; } = new();

    public ClaimsPrincipal ToClaimsPrincipal() => new(new ClaimsIdentity(new Claim[]
    {
        new (ClaimTypes.Name, Username),
        new (ClaimTypes.Hash, Password)
    }.Concat(Roles.Select(r => new Claim(ClaimTypes.Role, r)).ToArray()),
    "BlazorSchool"));

    public static User FromClaimsPrincipal(ClaimsPrincipal principal) => new()
    {
        Username = principal.FindFirstValue(ClaimTypes.Name),
        Password = principal.FindFirstValue(ClaimTypes.Hash),
        Roles = principal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList()
    };
}