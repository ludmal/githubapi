using Newtonsoft.Json;

namespace GithubApi.Services;

/// <summary>
/// Generic User detail model
/// </summary>
public class UserDetailsModel
{
    public string Name { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public int NumberOfFollowers { get; set; }
    public int NumberOfPublicRepo { get; set; }
    public int AvgFollowersPerRepo { get; set; } //We only need this property in the generic user model. Hence not in the Github Usermodel
}
    
