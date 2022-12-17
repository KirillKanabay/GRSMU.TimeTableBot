using GRSMU.Bot.Common.Telegram.Brokers.Contexts;

namespace GRSMU.Bot.Web.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class TelegramAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITelegramRequestContext _context;

        public TelegramAuthenticationMiddleware(RequestDelegate next, ITelegramRequestContext context)
        {
            _next = next;
            _context = context;
        }

        public Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path == "")
            {

            }

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class TelegramAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseTelegramAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TelegramAuthenticationMiddleware>();
        }
    }
}
