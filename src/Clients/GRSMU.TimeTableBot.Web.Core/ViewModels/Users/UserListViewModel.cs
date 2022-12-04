using GRSMU.Bot.Common.Web.ViewModels;
using GRSMU.Bot.Common.Web.ViewModels.Contracts;

namespace GRSMU.TimeTableBot.Web.Core.ViewModels.Users
{
    public class UserListViewModel : IPagingSupported
    {
        public List<UserViewModel> Users { get; set; }

        public PagingViewModel Paging { get; set; }
    }
}
