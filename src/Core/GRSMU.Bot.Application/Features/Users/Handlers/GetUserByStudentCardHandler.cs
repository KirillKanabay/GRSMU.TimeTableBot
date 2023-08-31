using GRSMU.Bot.Common.Broker.RequestHandlers;
using GRSMU.Bot.Common.Models.Messages;
using GRSMU.Bot.Domain.Users.Dtos;

namespace GRSMU.Bot.Application.Features.Users.Handlers;

public class GetUserByStudentCardHandler : CommandHandlerBase<>
{
    
}

public class GetUserByStudentCardRequestMessage : CommandMessageBase<ResponseStudentDto>
{

}