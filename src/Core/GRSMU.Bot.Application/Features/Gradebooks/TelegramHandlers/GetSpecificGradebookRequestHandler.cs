using AutoMapper;
using GRSMU.Bot.Application.Features.Gradebooks.Helpers;
using GRSMU.Bot.Common.Telegram.Brokers.Contexts;
using GRSMU.Bot.Common.Telegram.Brokers.Handlers;
using GRSMU.Bot.Common.Telegram.Data;
using GRSMU.Bot.Common.Telegram.Extensions;
using GRSMU.Bot.Common.Telegram.Immutable;
using GRSMU.Bot.Common.Telegram.Models.Messages;
using GRSMU.Bot.Data.Gradebooks.Contracts;
using GRSMU.Bot.Domain.Gradebooks.Dtos;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace GRSMU.Bot.Application.Features.Gradebooks.TelegramHandlers;

public class GetSpecificGradebookRequestHandler : SimpleTelegramRequestHandlerBase<GetSpecificGradebookRequestMessage>
{
    private readonly IGradebookRepository _gradebookRepository;
    private readonly IMapper _mapper;
    private readonly GradebookPresenter _presenter;

    public GetSpecificGradebookRequestHandler(ITelegramBotClient client, ITelegramRequestContext context, IGradebookRepository gradebookRepository, IMapper mapper, GradebookPresenter presenter) : base(client, context)
    {
        _gradebookRepository = gradebookRepository ?? throw new ArgumentNullException(nameof(gradebookRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
    }

    protected override async Task ExecuteAsync(GetSpecificGradebookRequestMessage request, CancellationToken cancellationToken)
    {
        var user = Context.User;

        var lastMessageId = user.LastBotMessageId;

        var gradebookRecord = await _gradebookRepository.GetByUserAsync(user.MongoId);
        var gradebook = _mapper.Map<GradebookDto>(gradebookRecord);
        var discipline = gradebook.Disciplines.First(x => x.Id.Equals(request.Data));

        var message = _presenter.GetDisciplineString(discipline);

        var markup = GetReplyMarkup(lastMessageId.ToString());

        await Client.EditMessageTextAsync
        (
            user.ChatId,
            lastMessageId.Value,
            text: message,
            replyMarkup: markup,
            cancellationToken: cancellationToken
        );
    }

    private InlineKeyboardMarkup GetReplyMarkup(string lastMessageId)
    {
        return new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData
        (
            text: "Назад",
            callbackData: CallbackDataProcessor.CreateCallbackData(new CallbackData
            {
                Data = lastMessageId,
                Handler = CommandKeys.Gradebook.SpecificGradebookKeyboard
            })
        ));
    }
}

public class GetSpecificGradebookRequestMessage : TelegramCommandMessageBase
{
    public string Data { get; set; }
}