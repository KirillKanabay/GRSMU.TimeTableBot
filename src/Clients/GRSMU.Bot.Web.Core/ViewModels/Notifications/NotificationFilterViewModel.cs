namespace GRSMU.Bot.Web.Core.ViewModels.Notifications;

public class NotificationFilterViewModel
{
    public List<string> GroupIds { get; set; }

    public List<string> CoursesIds { get; set; }

    public List<string> FacultyIds { get; set; }

    public bool IsBroadcast { get; set; }
}