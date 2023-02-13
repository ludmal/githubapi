using AutoMapper;
using GithubApi.RequestHandlers.GetUsers;
using GithubApi.Services;
using Moq;
using Shouldly;
using Xunit;

namespace GithubApi.Tests;

//TODO: We need more Unit test here but was lazy. :D
public class HandlerTests
{
    private readonly IMapper _mapper;
    private readonly Mock<GetUserRequestHandler> _mockGetUserRequestHandler;
    public HandlerTests()
    {
        _mockGetUserRequestHandler = new Mock<GetUserRequestHandler>();
        var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new GetUsersProfile()));
        _mapper = new Mapper(mapperConfiguration);
    }

    [Fact]
    public async Task when_valid_request_should_return_response()
    {
        var responseUsers = new List<UserDetailsModel>()
        {
            new()
            {
                Name = "mark",
                Company = "apple",
                Login = "mark",
                NumberOfFollowers = 12,
                NumberOfPublicRepo = 20
            }
        };

        var response = new GetUsersResponse(responseUsers);
        var request = new GetUserRequest()
        {
            Users = new List<string>()
            {
                "ludmal", "mark"
            }
        };

        // Arrange
        _mockGetUserRequestHandler.Setup(s => s.Handle(request, CancellationToken.None))
            .ReturnsAsync(response);

        // Act
        var result = await _mockGetUserRequestHandler.Object.Handle(request, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.UserDetails.Count().ShouldBe(2);
    }
}