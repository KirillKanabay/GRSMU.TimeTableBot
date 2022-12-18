using AutoMapper;
using GRSMU.Bot.Common.Telegram.Models;
using GRSMU.Bot.Data.Users.Contracts.Filters;
using GRSMU.Bot.Data.Users.Documents;
using GRSMU.Bot.Domain.Users.Dtos;
using GRSMU.Bot.Domain.Users.Dtos.Filters;
using MongoDB.Bson;
using Telegram.Bot.Types;

namespace GRSMU.Bot.Application.Features.Users.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDocument, UserDto>().ReverseMap();
            CreateMap<UserFilterDto, UserFilter>();

            CreateMap<UserDocument, TelegramUser>()
                .ForMember(x => x.MongoId, opt => opt.MapFrom(x => x.Id.ToString()))
                .ReverseMap()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => ObjectId.Parse(x.MongoId)));

            CreateMap<User, UserDocument>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.TelegramId, opt => opt.MapFrom(x => x.Id.ToString()));
        }
    }
}
