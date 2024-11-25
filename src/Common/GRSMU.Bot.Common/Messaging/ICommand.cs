using GRSMU.Bot.Common.Models;
using MediatR;

namespace GRSMU.Bot.Common.Messaging;

public interface ICommand : IRequest<ExecutionResult>, IRequestBase 
{}

public interface ICommand<TResponse> : IRequest<ExecutionResult<TResponse>>, IRequestBase
{}