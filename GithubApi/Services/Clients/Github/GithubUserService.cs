using AutoMapper;
using Newtonsoft.Json;

namespace GithubApi.Services.Clients.Github;

public class GithubUserService : IUserService 
{
    private readonly ILogger<GithubUserService> _logger;
    private readonly HttpClient _client;
    private readonly IMapper _mapper;

    public GithubUserService(ILogger<GithubUserService> logger, HttpClient client, IMapper mapper)
    {
        _logger = logger;
        _client = client;
        _mapper = mapper;
    }
        
    public async Task<IEnumerable<UserDetailsModel>> GetUserDetails(IEnumerable<string> users)
    {
        var userList = new List<UserDetailsModel>();
       
        //We use Parallelism for better optimization of the api calls. Typical we could have used Task.WhenAll as well. 
         await Parallel.ForEachAsync(users, new ParallelOptions() { MaxDegreeOfParallelism = 3 }, async (user, cancellationToken) =>
         {
             try
             {
                 var result = await _client.GetAsync($"users/{user}", cancellationToken);
                 _logger.LogInformation("Requesting data for user: {@User}", user);
                 
                 if(result.IsSuccessStatusCode) 
                     userList.Add(_mapper.Map<UserDetailsModel>(JsonConvert.DeserializeObject<GitUserDetailsModel>(await result.Content.ReadAsStringAsync())));
                 
                 if(!result.IsSuccessStatusCode)
                    _logger.LogInformation("Unable to get user information for user:{@User} with error code:{@ErrorCode}", user, result.StatusCode);
             }
             catch (Exception e)
             {
                 //We do nothing and continue to other user
                 _logger.LogError(e, "Unable to get user information for user {@User}", user);
             }
             
         });

        return userList;
    }
}