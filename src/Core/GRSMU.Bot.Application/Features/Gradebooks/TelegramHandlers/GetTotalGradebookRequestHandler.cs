using AutoMapper;
using GRSMU.Bot.Application.Features.Gradebooks.Helpers;
using GRSMU.Bot.Common.Telegram.Brokers.Contexts;
using GRSMU.Bot.Common.Telegram.Brokers.Handlers;
using GRSMU.Bot.Common.Telegram.Extensions;
using GRSMU.Bot.Common.Telegram.Models.Messages;
using GRSMU.Bot.Data.Gradebooks.Contracts;
using GRSMU.Bot.Domain.Gradebooks.Dtos;
using Telegram.Bot;

namespace GRSMU.Bot.Application.Features.Gradebooks.TelegramHandlers;

public class GetTotalGradebookRequestHandler : SimpleTelegramRequestHandlerBase<GetTotalGradebookRequestMessage>
{
    private readonly IMapper _mapper;
    private readonly IGradebookRepository _gradebookRepository;
    private readonly GradebookPresenter _presenter;

    public GetTotalGradebookRequestHandler(ITelegramBotClient client, ITelegramRequestContext context, IMapper mapper, IGradebookRepository gradebookRepository, GradebookPresenter presenter) : base(client, context)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _gradebookRepository = gradebookRepository ?? throw new ArgumentNullException(nameof(gradebookRepository));
        _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
    }

    protected override async Task ExecuteAsync(GetTotalGradebookRequestMessage request, CancellationToken cancellationToken)
    {
        var user = Context.User;

        var gradebookRecord = await _gradebookRepository.GetByUserAsync(user.MongoId);
        var gradebook = _mapper.Map<GradebookDto>(gradebookRecord);

        var message = _presenter.GetTotalGradebookString(gradebook);

        await Client.SendTextMessage(user, message);
    }
}

public class GetTotalGradebookRequestMessage : TelegramCommandMessageBase
{
}