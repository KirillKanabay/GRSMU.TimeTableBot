using AutoMapper;
using GRSMU.Bot.Data.Common.Documents;
using GRSMU.Bot.Logic.Dtos;

namespace GRSMU.Bot.Logic.Mappings;

public class CommonProfile : Profile
{
    public CommonProfile()
    {
        CreateMap<LookupDocument, LookupDto>();
    }
}