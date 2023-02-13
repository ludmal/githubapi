using System.Threading.Tasks.Dataflow;
using AutoMapper;
using GithubApi.Controllers;
using GithubApi.Services;
using MediatR;

namespace GithubApi.RequestHandlers.GetUsers;

/// <summary>
/// This is responsible for handling the request and returning the response
/// </summary>
public class GetUserRequestHandler : IRequestHandler<GetUserRequest, GetUsersResponse>
{
    private readonly ILogger<GetUserRequestHandler> _logger;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public GetUserRequestHandler(ILogger<GetUserRequestHandler> logger, IUserService userService, IMapper mapper)
    {
        _logger = logger;
        _userService = userService;
        _mapper = mapper;
    }
    
    //Simply sanitize the list by removing empty and with unique usernames
    private Func<IEnumerable<string>, IEnumerable<string>> SanitizeList => (list) => list.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();  
    
    public async Task<GetUsersResponse> Handle(GetUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            //We must log the request/response at least information level
            _logger.LogInformation("{@Request} received", request);
            var users = await _userService.GetUserDetails(SanitizeList(request.Users));
            var response = new GetUsersResponse(users.OrderBy(_ => _.Name));
            _logger.LogInformation("{@Response} sent", response);
            return response;
        }
        catch (Exception e)
        {
            //We log the error and throw. It is good practise to throw the exceptions in the handler.
            _logger.LogError(e, "Unable to get users from the server. User: {@Users}", string.Join(",", request.Users));
            throw;
        }
        
    }
}