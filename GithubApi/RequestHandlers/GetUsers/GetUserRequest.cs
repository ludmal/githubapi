using MediatR;

namespace GithubApi.RequestHandlers.GetUsers;

/// <summary>
/// Get user request with user list
/// </summary>
public class GetUserRequest : IRequest<GetUsersResponse>
{
    public IEnumerable<string> Users { get; set; } = Enumerable.Empty<string>();
}