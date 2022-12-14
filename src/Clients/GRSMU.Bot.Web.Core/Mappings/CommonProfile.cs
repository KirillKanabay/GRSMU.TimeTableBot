using AutoMapper;
using GRSMU.Bot.Common.Web.ViewModels;
using GRSMU.Bot.Common.Models;

namespace GRSMU.Bot.Web.Core.Mappings
{
    public class CommonProfile : Profile
    {
        public CommonProfile()
        {
            CreateMap<PagingModel, PagingViewModel>();
        }
    }
}
