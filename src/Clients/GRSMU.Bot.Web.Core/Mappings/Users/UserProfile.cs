using AutoMapper;
using GRSMU.Bot.Domain.Users.Dtos;
using GRSMU.Bot.Web.Core.ViewModels.Users;

namespace GRSMU.Bot.Web.Core.Mappings.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, UserViewModel>();
        }
    }
}
