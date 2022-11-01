namespace GRSMU.TimeTableBot.Common.Contexts;

public interface IUserContext
{
    string TelegramId { get; set; }

    string FirstName { get; set; }

    string LastName { get; set; }

    string Username { get; set; }
}