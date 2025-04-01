using BlogApp.Data.Helpers.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace BlogApp.Infrastructure
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {

            switch (exception)
            {
                case NotFoundException:
                    httpContext.Response.Redirect("/Error/NotFound");
                    break;
                case UnauthorizedException:
                    httpContext.Response.Redirect("/Error/AccessDenied");
                    break;
                default:
                    httpContext.Response.Redirect("/Error/ServerError");
                    break;
            }

            return ValueTask.FromResult(true);
        }
    }
}
