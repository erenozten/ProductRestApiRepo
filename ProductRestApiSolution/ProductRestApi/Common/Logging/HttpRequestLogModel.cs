namespace ProductRestApi.Common.Logging;

public class HttpRequestLogModel
{
    public string Method { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string Query { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}