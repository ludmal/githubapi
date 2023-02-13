using Newtonsoft.Json;

namespace GithubApi.Services.Clients.Github;

/// <summary>
/// Github specific user model
/// </summary>
public class GitUserDetailsModel
{
    [JsonProperty("id")]
    public long Id { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
    [JsonProperty("login")]
    public string Login { get; set; } = string.Empty;
    [JsonProperty("company")]
    public string? Company { get; set; }
    [JsonProperty("followers")]
    public int NumberOfFollowers { get; set; }
    [JsonProperty("public_repos")]
    public int NumberOfPublicRepo { get; set; }
}