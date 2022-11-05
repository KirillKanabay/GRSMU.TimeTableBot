using GRSMU.TimeTableBot.Common.Broker.Handlers;
using GRSMU.TimeTableBot.Common.Models.Responses;
using GRSMU.TimeTableBot.Domain.Users.Dtos;
using GRSMU.TimeTableBot.Domain.Users.Requests;

namespace GRSMU.TimeTableBot.Application.Users.Handlers
{
    public class GetUsersRequestHandler : RequestHandlerBase<GetUsersRequestMessage, ItemPagedResponse<UserDto>>
    {

        public GetUsersRequestHandler()
        {
            
        }

        protected override Task<ItemPagedResponse<UserDto>> ExecuteAsync(GetUsersRequestMessage request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
