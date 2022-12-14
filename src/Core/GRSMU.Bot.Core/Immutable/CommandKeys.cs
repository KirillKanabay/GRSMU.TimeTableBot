namespace GRSMU.Bot.Core.Immutable;

public static class CommandKeys
{
    public const string SetDefaultMenu = "В главное меню";
    public const string Start = "/start";
    public const string ShowTimeTableCommand = "Показать расписание";
    public const string ChangeSettingsCommand = "Изменить настройки";

    public static class Reports
    {
        public const string Report = "Пожаловаться";
        public const string Cancel = "Report_Cancel";
    }

    public static class Registrators
    {
        public const string Run = "Настройки профиля";
        public const string Course = "CourseRegistator";
        public const string Faculty = "FacultyRegistator";
        public const string Group = "GroupRegistator";
        public const string Back = nameof(Back);
    }

    public static class TimeTable
    {
        public const string Today = "На сегодня";
        public const string Tomorrow = "На завтра";
        public const string Week = "На всю неделю";
        public const string NextWeek = "На следующую неделю";
    }
}