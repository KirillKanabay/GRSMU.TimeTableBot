using GRSMU.Bot.Logic.Dtos;
using MediatR;

namespace GRSMU.Bot.Logic.Features.Users.Events;

public record StudentCardUpdatedEvent(string UserId, StudentCardIdDto StudentCard) : INotification;