using AutoMapper;
using GRSMU.Bot.Logic.Dtos;
using GRSMU.Bot.Web.Api.Models;

namespace GRSMU.Bot.Web.Api.Mappings;

public class CommonProfile : Profile
{
    public CommonProfile()
    {
        CreateMap<LookupDto, LookupModel>();
    }
}