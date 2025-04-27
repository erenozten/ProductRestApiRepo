using ProductRestApi.Common.Logging;

namespace ProductRestApi.Middlewares;

using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ProductRestApi.Common.Helpers;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        context.Request.EnableBuffering(); 
        var requestBody = await ReadRequestBody(context.Request);

        var requestLogModel = new HttpRequestLogModel
        {
            Method = context.Request.Method,
            Path = context.Request.Path,
            Query = context.Request.Query.ToString() ?? string.Empty,
            Body = JsonHelper.PrettyPrintJson(requestBody) ?? string.Empty
        };

        _logger.LogInformation("Request Details: {@Request}", requestLogModel);

        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await _next(context);

        responseBody.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(responseBody).ReadToEndAsync();

        var responseLogModel = new HttpResponseLogModel();

        _logger.LogInformation("Response Details: {@MyResponse}", new HttpResponseLogModel()
        {
            StatusCode = context.Response.StatusCode,
            Body = JsonHelper.PrettyPrintJson(responseText) ?? string.Empty
            /*
             Gpt: Yani sadece MyStatus ve MyBody alanlarını sen verdin. Fakat logda şunlar da var: RequestPath RequestId ConnectionId SourceContext Peki bunlar nereden geldi?
             Bunlar SEQ'in ve ILogger altyapısının otomatik eklediği metadata’lardır. ASP.NET Core, Serilog (veya diğer ILogger sağlayıcıları) ile entegreyken şu bilgileri otomatik log entry’sine iliştirir:
                   RequestPath	Middleware pipeline'da o an işlenen HttpContext.Request.Path'i temsil eder.
                   RequestId	Her request’e özel GUID benzeri bir kimliktir. Trace amaçlıdır.
                   ConnectionId	Client bağlantısına özel ID’dir. Genellikle Kestrel verir.
                   SourceContext	Log mesajının hangi class’tan (veya middleware’den) geldiğini gösterir. ILogger<T>'nin T tipi.
             */
        });

        responseBody.Seek(0, SeekOrigin.Begin); 
        await responseBody.CopyToAsync(originalBodyStream); 

        context.Response.Body = originalBodyStream; 
    }


    private async Task<string> ReadRequestBody(HttpRequest request)
    {
        request.Body.Position = 0;
        using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        request.Body.Position = 0;
        return string.IsNullOrWhiteSpace(body) ? "[empty]" : body;
    }

    private async Task<string> ReadResponseBody(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        var text = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);
        return string.IsNullOrWhiteSpace(text) ? "[empty]" : text;
    }
}


// 9997{{99////()=()//ØØØØøØooıiş{{[{] asd<>!!(==)