using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;

namespace AuthorizeOnRoute.Data;

public class BlazorSchoolUserService
{
    private readonly ProtectedLocalStorage _protectedLocalStorage;
    private readonly string _blazorSchoolStorageKey = "blazorSchoolIdentity";

    public BlazorSchoolUserService(ProtectedLocalStorage protectedLocalStorage)
    {
        _protectedLocalStorage = protectedLocalStorage;
    }

    public User? LookupUserInDatabase(string username, string password)
    {
        var usersFromDatabase = new List<User>()
        {
            new()
            {
                Username = "blazorschool_normal",
                Password = "blazorschool",
                Roles =new()
                {
                    "normal_user"
                }
            },
            new()
            {
                Username = "blazorschool_paid",
                Password = "blazorschool",
                Roles =new()
                {
                    "normal_user",
                    "paid_user"
                }
            },
            new()
            {
                Username = "blazorschool_admin",
                Password = "blazorschool",
                Roles =new()
                {
                    "normal_user",
                    "paid_user",
                    "admin"
                }
            },
            new()
            {
                Username = "blazorschool_adult_user",
                Password = "blazorschool",
                Roles =new()
                {
                    "normal_user"
                },
                Age = 22
            },
            new()
            {
                Username = "blazorschool_adult_admin",
                Password = "blazorschool",
                Roles =new()
                {
                    "normal_user",
                    "admin"
                },
                Age = 22
            }
        };

        var foundUser = usersFromDatabase.SingleOrDefault(u => u.Username == username && u.Password == password);

        return foundUser;
    }

    public async Task PersistUserToBrowserAsync(User user)
    {
        string userJson = JsonConvert.SerializeObject(user);
        await _protectedLocalStorage.SetAsync(_blazorSchoolStorageKey, userJson);
    }

    public async Task<User?> FetchUserFromBrowserAsync()
    {
        try
        {
            var storedUserResult = await _protectedLocalStorage.GetAsync<string>(_blazorSchoolStorageKey);

            if (storedUserResult.Success && !string.IsNullOrEmpty(storedUserResult.Value))
            {
                var user = JsonConvert.DeserializeObject<User>(storedUserResult.Value);

                return user;
            }
        }
        catch (InvalidOperationException)
        {
        }

        return null;
    }

    public async Task ClearBrowserUserDataAsync() => await _protectedLocalStorage.DeleteAsync(_blazorSchoolStorageKey);
}