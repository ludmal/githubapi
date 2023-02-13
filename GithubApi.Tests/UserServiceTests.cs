using AutoMapper;
using GithubApi.RequestHandlers.GetUsers;
using GithubApi.Services;
using GithubApi.Services.Clients.Github;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using Xunit;

namespace GithubApi.Tests;

public class UserServiceTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IUserService> _mockUserService;

    public UserServiceTests()
    {
        _mockUserService = new Mock<IUserService>();
        var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new GetUsersProfile()));
        _mapper = new Mapper(mapperConfiguration);
    }

    [Fact]
    public async Task when_valid_request_should_return_response()
    {
        var response = new List<UserDetailsModel>()
        {
            new UserDetailsModel()
            {
                Name = "mark",
                Company = "apple",
                Login = "mark",
                NumberOfFollowers = 12,
                NumberOfPublicRepo = 20
            }
        };

        var request = new List<string>()
        {
            "ludmal", "mark"
        };

        // Arrange
        _mockUserService.Setup(s => s.GetUserDetails(request))
            .ReturnsAsync(response);

        // Act
        var result = (await _mockUserService.Object.GetUserDetails(request)).ToList();

        // Assert
        result.ShouldNotBeNull();
        result.Count().ShouldBeGreaterThan(0);
    }

    [Fact]
    public void should_valid_followers_repos_should_return_valid_average_followers_per_repo()
    {
        var githubUser = new GitUserDetailsModel()
        {
            Name = "mark",
            Company = "apple",
            Login = "mark",
            NumberOfFollowers = 10,
            NumberOfPublicRepo = 5
        };

        var genericUser = _mapper.Map<UserDetailsModel>(githubUser);
        
        genericUser.AvgFollowersPerRepo.ShouldBe(2);
    }
}