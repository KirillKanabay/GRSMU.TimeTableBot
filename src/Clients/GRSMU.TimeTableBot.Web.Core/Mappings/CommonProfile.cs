using AutoMapper;
using GRSMU.Bot.Common.Web.ViewModels;
using GRSMU.TimeTableBot.Common.Models;

namespace GRSMU.TimeTableBot.Web.Core.Mappings
{
    public class CommonProfile : Profile
    {
        public CommonProfile()
        {
            CreateMap<PagingModel, PagingViewModel>();
        }
    }
}
