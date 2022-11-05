using AutoMapper;
using GRSMU.TimeTableBot.Data.Users.Contracts.Filters;
using GRSMU.TimeTableBot.Data.Users.Documents;
using GRSMU.TimeTableBot.Domain.Users.Dtos;
using GRSMU.TimeTableBot.Domain.Users.Dtos.Filters;

namespace GRSMU.TimeTableBot.Application.Users.Mappings
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
