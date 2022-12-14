using AutoMapper;
using GRSMU.Bot.Common.Contexts;
using GRSMU.Bot.Data.Users.Documents;
using MongoDB.Bson;
using Telegram.Bot.Types;

namespace GRSMU.Bot.Core.Mappings
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<UserDocument, UserContext>()
                .ForMember(x => x.MongoId, opt => opt.MapFrom(x => x.Id.ToString()))
                .ReverseMap()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => ObjectId.Parse(x.MongoId)));

            CreateMap<User, UserDocument>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.TelegramId, opt => opt.MapFrom(x => x.Id.ToString()));
        }
    }
}
