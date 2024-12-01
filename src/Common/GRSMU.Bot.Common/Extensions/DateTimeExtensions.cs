namespace GRSMU.Bot.Common.Extensions;

public static class DateTimeExtensions
{
    public static DateTime StartOfDay(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day);
    }

    public static DateTime EndOfDay(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
    }

    public static DateTime StartOfWeek(this DateTime date)
    {
        return date.AddDays(-(int)date.DayOfWeek + (int)DayOfWeek.Monday).StartOfDay();
    }

    public static DateTime EndOfWeek(this DateTime date)
    {
        return date.StartOfWeek().AddDays(6).EndOfDay();
    }
}