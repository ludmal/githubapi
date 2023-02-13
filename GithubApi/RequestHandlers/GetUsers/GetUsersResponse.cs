using GithubApi.Services;

namespace GithubApi.RequestHandlers.GetUsers;

public class GetUsersResponse
{
    public GetUsersResponse(IEnumerable<UserDetailsModel> userDetails)
    {
        this.UserDetails = userDetails;
    }
    public IEnumerable<UserDetailsModel> UserDetails { get; set; }
}