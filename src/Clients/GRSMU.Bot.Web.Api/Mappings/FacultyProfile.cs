using AutoMapper;
using GRSMU.Bot.Logic.Features.Faculty.Dtos;
using GRSMU.Bot.Web.Api.Models.Faculty.Responses;

namespace GRSMU.Bot.Web.Api.Mappings;

public class FacultyProfile : Profile
{
    public FacultyProfile()
    {
        CreateMap<FacultyFullLookupDto, FacultyFullLookupResponse>();
    }
}