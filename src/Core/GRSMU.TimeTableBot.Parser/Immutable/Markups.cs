using Telegram.Bot.Types.ReplyMarkups;

namespace GRSMU.TimeTableBot.Core.Immutable
{
    public static class Markups
    {
        public static readonly ReplyKeyboardMarkup DefaultMarkup = new(new[]
        {
            new KeyboardButton[] { CommandKeys.ShowTimeTableCommand },
            new KeyboardButton[] { CommandKeys.Reports.Report, CommandKeys.Registrators.Run },
        })
        {
            ResizeKeyboard = true
        };

        public static readonly ReplyKeyboardMarkup TimeTableMarkup = new(new[]
        {
            new KeyboardButton[] { CommandKeys.TimeTable.Today, CommandKeys.TimeTable.Week },
            new KeyboardButton[] { CommandKeys.TimeTable.Tomorrow, CommandKeys.TimeTable.NextWeek },
            new KeyboardButton[] { CommandKeys.SetDefaultMenu },
        })
        {
            ResizeKeyboard = true
        };
    }
}
