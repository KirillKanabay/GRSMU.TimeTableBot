using AutoMapper;
using GRSMU.Bot.Data.Gradebooks.Documents;
using GRSMU.Bot.Domain.Gradebooks.Dtos;

namespace GRSMU.Bot.Application.Features.Gradebooks.Mappings
{
    public class GradebookProfile : Profile
    {
        public GradebookProfile()
        {
            CreateMap<GradebookDocument, GradebookDto>().ReverseMap();
            CreateMap<DisciplineDocument, DisciplineDto>().ReverseMap();
            CreateMap<MarkDocument, MarkDto>().ReverseMap();
        }
    }
}
