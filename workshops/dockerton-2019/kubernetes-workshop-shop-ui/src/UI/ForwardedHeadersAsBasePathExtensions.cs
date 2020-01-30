using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace DFDS.UI
{
    public static class ForwardedHeadersAsBasePathExtensions
    {
        public static IApplicationBuilder UseForwardedHeadersAsBasePath(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ForwardedHeaderBasePath>();
        }

        public class ForwardedHeaderBasePath : IMiddleware
        {
            public async Task InvokeAsync(HttpContext context, RequestDelegate next)
            {
                if (context.Request.Headers.TryGetValue("X-Forwarded-Prefix", out var prefix))
                {
                    //context.Request.Path = prefix + context.Request.Path;
                    context.Request.PathBase = new PathString(prefix);
                }
                if (context.Request.Headers.TryGetValue("X-Forwarded-Proto", out var protocol))
                {
                    context.Request.Scheme = protocol;
                }

                await next(context);
            }
        }
    }
}