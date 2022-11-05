using AutoMapper;
using GRSMU.TimeTableBot.Domain.Users.Dtos;
using GRSMU.TimeTableBot.Web.Core.ViewModels.Users;

namespace GRSMU.TimeTableBot.Web.Core.Mappings.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, UserViewModel>();
        }
    }
}
