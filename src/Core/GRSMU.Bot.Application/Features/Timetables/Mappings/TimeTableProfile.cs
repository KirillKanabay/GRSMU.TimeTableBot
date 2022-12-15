using AutoMapper;
using GRSMU.Bot.Common.Models;
using GRSMU.Bot.Data.TimeTables.Documents;
using GRSMU.Bot.Domain.Timetables.Dtos;

namespace GRSMU.Bot.Application.Timetables.Mappings
{
    public class TimeTableProfile : Profile
    {
        public TimeTableProfile()
        {
            CreateMap<TimeTableParsedModel, TimeTableDocument>();
            CreateMap<TimeTableLineParsedModel, TimeTableLineDocument>();

            CreateMap<TimeTableParsedModel, TimeTableDto>();
            CreateMap<TimeTableLineParsedModel, TimeTableLineDto>();

            CreateMap<TimeTableDocument, TimeTableDto>();
            CreateMap<TimeTableLineDocument, TimeTableLineDto>();
        }
    }
}
