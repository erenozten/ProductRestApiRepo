using ProductRestApi.Common.Constants;
using ProductRestApi.Common.Logging;
using ProductRestApi.Common.Responses;

namespace ProductRestApi.Middlewares;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // _logger.LogError(ConstMessages.LogStartFinishDash);
        // _logger.LogInformation( " Started Request: {Path}", context.Request.Path);

        try
        {
            await _next(context);
            _logger.LogInformation(LoggingTemplates.RequestPath, context.Request.Path);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(LoggingTemplates.RequestPath, context.Request.Path);
            _logger.LogWarning(LoggingTemplates.RequestMethod, context.Request.Method);
            _logger.LogError(ex, $"Finished Request with Error -> {ex.Message}");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = GenericApiResponse<object>.Fail(
                null,
                StatusCodes.Status500InternalServerError,
                ex.Message,
                ConstMessages.INTERNAL_SERVER_ERROR_Description
            );

            await context.Response.WriteAsJsonAsync(response);
        }
        finally
        {
        }
    }
}