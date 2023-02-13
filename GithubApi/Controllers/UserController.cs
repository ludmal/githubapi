using System.Net.Http.Headers;
using GithubApi.RequestHandlers.GetUsers;
using GithubApi.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GithubApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private readonly IMediator _mediator;

    //Good practise to keep the thin controllers. We don't need logging here since we can use UseRequestLogging middleware built in.
    [HttpPost(Name = "retrieveUsers")]
    public async Task<IActionResult> Post(GetUserRequest request) => this.Ok(await _mediator.Send(request));
}