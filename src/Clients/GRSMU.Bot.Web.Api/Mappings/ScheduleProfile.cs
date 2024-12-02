using AutoMapper;
using GRSMU.Bot.Logic.Features.Schedule.Dtos;
using GRSMU.Bot.Web.Api.Models.Schedule.Responses;

namespace GRSMU.Bot.Web.Api.Mappings;

public class ScheduleProfile : Profile
{
    public ScheduleProfile()
    {
        CreateMap<ScheduleDayDto, ScheduleDayModel>();
        CreateMap<ScheduleItemDto, ScheduleItemModel>();
    }
}