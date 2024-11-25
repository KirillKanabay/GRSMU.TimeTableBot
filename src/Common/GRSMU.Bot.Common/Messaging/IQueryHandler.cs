using GRSMU.Bot.Common.Models;
using MediatR;

namespace GRSMU.Bot.Common.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, ExecutionResult<TResponse>>
    where TQuery : IQuery<TResponse>
{}