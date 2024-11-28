using GRSMU.Bot.Common.Messaging;
using GRSMU.Bot.Logic.Features.Users.Dtos;

namespace GRSMU.Bot.Logic.Features.Users.Commands.GetOrCreateUser;

public record GetOrCreateUserCommand : ICommand<UserDto>
{
    public required string TelegramId { get; set; }

    public long ChatId { get; set; }

    public string? TelegramFirstName { get; set; }

    public string? TelegramLastName { get; set; }

    public string? TelegramUsername { get; set; }
}