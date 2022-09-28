using System.Security.Claims;

namespace BasicAuthentication.Data;

public class User
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";

    public ClaimsPrincipal ToClaimsPrincipal() => new(new ClaimsIdentity(new Claim[]
    {
        new (ClaimTypes.Name, Username),
        new (ClaimTypes.Hash, Password),
    }, "BlazorSchool"));

    public static User FromClaimsPrincipal(ClaimsPrincipal principal) => new()
    {
        Username = principal.FindFirstValue(ClaimTypes.Name),
        Password = principal.FindFirstValue(ClaimTypes.Hash)
    };
}