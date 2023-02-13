using GithubApi.Services.Clients.Github;

namespace GithubApi.Services;

/// <summary>
/// Generic service interface to get users
/// </summary>
public interface IUserService
{
    Task<IEnumerable<UserDetailsModel>> GetUserDetails(IEnumerable<string> users);
}