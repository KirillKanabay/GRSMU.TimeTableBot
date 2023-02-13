using MediatR;
using TelegramResponse = GRSMU.Bot.Common.Telegram.Models.Responses.TelegramResponse;

namespace GRSMU.Bot.Common.Telegram.Models.Messages;

public abstract class TelegramCommandMessageBase : IRequest<TelegramResponse>
{
}