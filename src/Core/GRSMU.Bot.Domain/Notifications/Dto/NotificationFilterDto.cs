using GRSMU.Bot.Domain.Notifications.Enums;

namespace GRSMU.Bot.Domain.Notifications.Dto
{
    public class NotificationFilterDto
    {
        public string Text { get; set; }

        public NotificationType Type { get; set; }
    }
}
