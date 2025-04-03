using BlogApp.Data.Constants;
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
                    httpContext.Response.Redirect(ControllerConstants.NotFoundEndpoint);
                    break;
                case UnauthorizedException:
                    httpContext.Response.Redirect(ControllerConstants.AccessDeniedEndpoint);
                    break;
                default:
                    httpContext.Response.Redirect(ControllerConstants.ServerErrorEndpoint);
                    break;
            }

            return ValueTask.FromResult(true);
        }
    }
}
