using AutoMapper;
using GRSMU.Bot.Data.Users.Contracts.Filters;
using GRSMU.Bot.Data.Users.Documents;
using GRSMU.Bot.Domain.Users.Dtos;
using GRSMU.Bot.Domain.Users.Dtos.Filters;

namespace GRSMU.Bot.Application.Users.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDocument, UserDto>().ReverseMap();
            CreateMap<UserFilterDto, UserFilter>();
        }
    }
}
