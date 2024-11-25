using AutoMapper;
using GRSMU.Bot.Data.Users.Documents;
using GRSMU.Bot.Logic.Features.Users.Dtos;

namespace GRSMU.Bot.Logic.Features.Users
{
    internal class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDocument, UserDto>();
        }
    }
}
