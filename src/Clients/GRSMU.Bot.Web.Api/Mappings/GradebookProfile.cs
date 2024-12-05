using AutoMapper;
using GRSMU.Bot.Logic.Dtos;
using GRSMU.Bot.Logic.Features.Gradebook.Dtos;
using GRSMU.Bot.Web.Api.Models.Gradebook;

namespace GRSMU.Bot.Web.Api.Mappings;

public class GradebookProfile : Profile
{
    public GradebookProfile()
    {
        CreateMap<GradebookDto, GradebookModel>();
        CreateMap<MarkDto, MarkModel>();
    }
}