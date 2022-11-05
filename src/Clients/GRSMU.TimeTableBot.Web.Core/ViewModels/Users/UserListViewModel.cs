using GRSMU.TimeTableBot.Common.Models;

namespace GRSMU.TimeTableBot.Web.Core.ViewModels.Users
{
    public class UserListViewModel
    {
        public List<UserViewModel> Users { get; set; }

        public PagingModel Paging { get; set; }
    }
}
