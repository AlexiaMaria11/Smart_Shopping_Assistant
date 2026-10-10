using SmartShoppingAssistant.BusinessLogic.Helpers;
using System.Net;

namespace SmartShoppingAssistant.Api.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var status = ex switch
            {
                KeyNotFoundException => HttpStatusCode.NotFound,
                UnauthorizedAccessException => HttpStatusCode.Forbidden,
                BusinessException or ArgumentException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };

            if (status == HttpStatusCode.InternalServerError)
                logger.LogError(ex, "Unhandled exception");
            else if (ex.InnerException is not null)
                // The user gets a friendly message; the real cause (e.g. an invalid AI key) goes to the log
                logger.LogWarning(ex.InnerException, "{Message}", ex.Message);

            context.Response.StatusCode = (int)status;
            await context.Response.WriteAsJsonAsync(new
            {
                message = status == HttpStatusCode.InternalServerError
                    ? "Something went wrong. Please try again."
                    : ex.Message
            });
        }
    }
}
