using GRSMU.TimeTableBot.Common.Models.RequestMessages;
using GRSMU.TimeTableBot.Common.Models.Responses;
using GRSMU.TimeTableBot.Domain.Users.Dtos;
using GRSMU.TimeTableBot.Domain.Users.Dtos.Filters;

namespace GRSMU.TimeTableBot.Domain.Users.Requests;

public class GetUsersRequestMessage : FilterPagedRequestMessage<UserFilterDto, ItemPagedResponse<UserDto>>
{

}