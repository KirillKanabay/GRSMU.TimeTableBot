using GRSMU.Bot.Common.Messaging;
using GRSMU.Bot.Logic.Features.Users.Dtos;

namespace GRSMU.Bot.Logic.Features.Users.Queries.GetById;

public record GetUserByIdQuery(string Id) : IQuery<UserDto>;