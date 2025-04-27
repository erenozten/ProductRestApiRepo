namespace ProductRestApi.Common.Logging;

public class HttpResponseLogModel
{
    public int StatusCode { get; set; }
    public string Body { get; set; } = string.Empty;
}