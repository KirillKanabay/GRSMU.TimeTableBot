namespace GRSMU.Bot.Common.Telegram.Immutable;

public static class CommandKeys
{
    public const string EmptyData = "_";
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

    public static class Gradebook
    {
        public const string Run = "Журнал отметок";
        public const string Total = "Общая успеваемость";
        public const string Specific = "По конкретному предмету";

        public static readonly string SpecificGradebook = "SG";
        public static readonly string SpecificGradebookKeyboard = "SGK";

        public static readonly string Back = $"{nameof(Gradebook)}_{nameof(Back)}";
        public static readonly string Cancel = $"{nameof(Gradebook)}_{nameof(Cancel)}";

        public static readonly string SetLogin = $"{nameof(Gradebook)}_{nameof(SetLogin)}";
        public static readonly string SetPassword = $"{nameof(Gradebook)}_{nameof(SetPassword)}";
    }
}