using AutoMapper;
using GRSMU.Bot.Data.Gradebooks.Documents;
using GRSMU.Bot.Logic.Features.Gradebook.Dtos;

namespace GRSMU.Bot.Logic.Features.Gradebook.Mappings;

public class GradebookProfile : Profile
{
    public GradebookProfile()
    {
        CreateMap<GradebookDocument, GradebookDto>().ReverseMap();
        CreateMap<MarkDocument, MarkDto>().ReverseMap();
    }
}