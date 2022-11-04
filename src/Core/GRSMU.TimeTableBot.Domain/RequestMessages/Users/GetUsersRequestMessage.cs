using GRSMU.TimeTableBot.Common.Models.RequestMessages;
using GRSMU.TimeTableBot.Common.Models.Responses;
using GRSMU.TimeTableBot.Domain.Dtos.Users;
using GRSMU.TimeTableBot.Domain.Dtos.Users.Filters;

namespace GRSMU.TimeTableBot.Domain.RequestMessages.Users;

public class GetUsersRequestMessage : FilterPagedRequestMessage<UserFilterDto, ItemPagedResponse<UserDto>>
{
    
}