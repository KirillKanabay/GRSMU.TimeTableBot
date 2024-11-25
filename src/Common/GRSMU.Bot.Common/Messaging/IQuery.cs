using GRSMU.Bot.Common.Models;
using MediatR;

namespace GRSMU.Bot.Common.Messaging;

public interface IQuery<TResponse> : IRequest<ExecutionResult<TResponse>>, IRequestBase
{}