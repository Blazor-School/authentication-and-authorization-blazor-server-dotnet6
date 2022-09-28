using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BasicAuthentication.Data;

public class BlazorSchoolAuthenticationStateProvider : AuthenticationStateProvider, IDisposable
{
    private readonly BlazorSchoolUserService _blazorSchoolUserService;
    public User CurrentUser { get; private set; } = new();

    public BlazorSchoolAuthenticationStateProvider(BlazorSchoolUserService blazorSchoolUserService)
    {
        _blazorSchoolUserService = blazorSchoolUserService;
        AuthenticationStateChanged += OnAuthenticationStateChangedAsync;
    }

    private async void OnAuthenticationStateChangedAsync(Task<AuthenticationState> task)
    {
        var authenticationState = await task;

        if (authenticationState is not null)
        {
            CurrentUser = User.FromClaimsPrincipal(authenticationState.User);
        }
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var principal = new ClaimsPrincipal();
        var user = await _blazorSchoolUserService.FetchUserFromBrowserAsync();

        if (user is not null)
        {
            var userInDatabase = _blazorSchoolUserService.LookupUserInDatabase(user.Username, user.Password);

            if (userInDatabase is not null)
            {
                principal = userInDatabase.ToClaimsPrincipal();
            }
        }

        return new(principal);
    }

    public async Task LoginAsync(string username, string password)
    {
        var principal = new ClaimsPrincipal();
        var user = _blazorSchoolUserService.LookupUserInDatabase(username, password);

        if(user is not null)
        {
            await _blazorSchoolUserService.PersistUserToBrowserAsync(user);
            principal = user.ToClaimsPrincipal();
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    public async Task LogoutAsync()
    {
        await _blazorSchoolUserService.ClearBrowserUserDataAsync();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new())));
    }

    public void Dispose() => AuthenticationStateChanged -= OnAuthenticationStateChangedAsync;
}
