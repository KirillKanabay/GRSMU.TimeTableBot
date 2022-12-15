using GRSMU.Bot.Common.Models.Messages;
using GRSMU.Bot.Common.Models.Responses;
using GRSMU.Bot.Domain.Users.Dtos;
using GRSMU.Bot.Domain.Users.Dtos.Filters;

namespace GRSMU.Bot.Domain.Users.Requests;

public class GetUsersRequestMessage : FilterPagedRequestMessage<UserFilterDto, ItemPagedResponse<UserDto>>
{

}