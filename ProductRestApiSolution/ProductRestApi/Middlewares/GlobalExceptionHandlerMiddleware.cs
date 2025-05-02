using ProductRestApi.Common.Constants;
using ProductRestApi.Common.Responses;
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
        _logger.LogInformation( "📥\n ---------------------------------------------------------------- Started Request: {RequestPath}", context.Request.Path);

        try
        {
            await _next(context);
            _logger.LogInformation( "📥\n ---------------------------------------------------------------- Finished Request: {RequestPath}", context.Request.Path);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Request failed for {Method} {Path}", context.Request.Method, context.Request.Path);
            _logger.LogError(ex, $" ---------------------------------------------------------------- Finished Request with Error -> {ex.Message}");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = GenericApiResponse<object>.Fail(
                null,
                StatusCodes.Status500InternalServerError,
                "Bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.",
                ConstMessages.INTERNAL_SERVER_ERROR_Description
            );

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}