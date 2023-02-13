using AutoMapper;
using GithubApi.Services;
using GithubApi.Services.Clients.Github;

namespace GithubApi.RequestHandlers.GetUsers;

public class GetUsersProfile : Profile
{
    //Simply to map the Github users to Generic user model
    public GetUsersProfile()
    {
        CreateMap<GitUserDetailsModel, UserDetailsModel>()
            .ForMember(s => s.AvgFollowersPerRepo, s => s.MapFrom(o =>
                o.NumberOfPublicRepo != 0
                    ? Math.Ceiling(Convert.ToDecimal(o.NumberOfFollowers / o.NumberOfPublicRepo))
                    : 0
            ));
    }
}