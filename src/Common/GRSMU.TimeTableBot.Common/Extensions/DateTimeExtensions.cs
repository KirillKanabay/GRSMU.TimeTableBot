namespace GRSMU.TimeTableBot.Common.Extensions;

public static class DateTimeExtensions
{
    public static DateTime StartOfWeek(this DateTime date)
    {
        return date.AddDays(-(int)date.DayOfWeek + (int)DayOfWeek.Monday);
    }

    public static DateTime EndOfWeek(this DateTime date)
    {
        return date.StartOfWeek().AddDays(6);
    }
}