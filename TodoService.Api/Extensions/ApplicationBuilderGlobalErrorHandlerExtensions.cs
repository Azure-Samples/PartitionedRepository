using Microsoft.AspNetCore.Builder;
using TodoService.Api.Middleware;

namespace TodoService.Api.Extensions
{
    public static class ApplicationBuilderGlobalErrorHandlerExtensions
    {
        public static IApplicationBuilder UseGlobalErrorHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalErrorHandlerMiddleware>();
        }
    }
}
