using AutoMapper;
using GRSMU.Bot.Data.Faculties.Documents;
using GRSMU.Bot.Logic.Dtos;

namespace GRSMU.Bot.Logic.Features.Faculty.Mappings;

public class FacultyProfile : Profile
{
    public FacultyProfile()
    {
        CreateMap<GroupDocument, LookupDto>()
            .ConstructUsing(src => new LookupDto(src.GroupId, src.GroupName));
    }
}