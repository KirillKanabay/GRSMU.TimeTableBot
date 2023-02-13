using AutoMapper;
using GRSMU.Bot.Application.Features.Notifications.Handlers;
using GRSMU.Bot.Domain.Notifications.Dto;
using GRSMU.Bot.Web.Core.ViewModels.Notifications;

namespace GRSMU.Bot.Web.Core.Mappings.Notifications;

public class NotificationProfile : Profile
{
    public NotificationProfile()
    {
        CreateMap<NotificationFilterViewModel, NotificationFilterDto>();

        CreateMap<NotificationViewModel, NotifyUsersRequestMessage>();
    }
}