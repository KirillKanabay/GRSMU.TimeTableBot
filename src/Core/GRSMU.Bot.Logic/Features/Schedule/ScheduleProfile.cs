using AutoMapper;
using GRSMU.Bot.Data.TimeTables.Documents;
using GRSMU.Bot.Logic.Features.Schedule.Dtos;

namespace GRSMU.Bot.Logic.Features.Schedule;

public class ScheduleProfile : Profile
{
    public ScheduleProfile()
    {
        CreateMap<ParsedScheduleDayDto, TimeTableDocument>()
            .ForMember(x => x.Lines, opt => opt.MapFrom(x => x.Items));
        CreateMap<ScheduleItemDto, TimeTableLineDocument>()
            .ReverseMap();
    }
}