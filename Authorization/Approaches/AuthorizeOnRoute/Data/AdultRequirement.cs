using Microsoft.AspNetCore.Authorization;

namespace AuthorizeOnRoute.Data;

public class AdultRequirement : IAuthorizationRequirement
{
    public int MinimumAgeToConsiderAnAdult { get; set; } = 18;
}