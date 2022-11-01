using System.Net;

namespace GRSMU.TimeTableBot.Common.Extensions
{
    public static class StringExtensions
    {
        public static Cookie CreateCookie(this string cookieString)
        {
            var eqIdx = cookieString.IndexOf('=');
            var name = cookieString.Substring(0, eqIdx);
            var value = cookieString.Substring(eqIdx + 1);

            return new Cookie(name, value);
        }
    }
}
