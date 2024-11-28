using AutoMapper;
using GRSMU.Bot.Domain.Users.Dtos;
using GRSMU.Bot.Web.Api.Models.User.Responses;

namespace GRSMU.Bot.Web.Api.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserDto, UserMeResponse>();
    }
}