using GRSMU.Bot.Common.Models;
using MediatR;

namespace GRSMU.Bot.Common.Messaging;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, ExecutionResult>
    where TCommand : ICommand
{}

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, ExecutionResult<TResponse>>
    where TCommand : ICommand<TResponse>
{}