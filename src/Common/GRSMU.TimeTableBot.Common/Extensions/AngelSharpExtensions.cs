using AngleSharp.Html.Dom;

namespace GRSMU.TimeTableBot.Common.Extensions
{
    public static class AngelSharpExtensions
    {
        public static string GetInputValue(this IHtmlFormElement form, string id)
        {
            var value = ((IHtmlInputElement)form.Children.FirstOrDefault(x => x.Id?.Equals(id) ?? false))?.Value ?? string.Empty;

            return value;
        }
    }
}
