using System.Net;

namespace ProductRestApi.Common.Extensions;

public static class HttpStatusExtensions
{
    public static string ToStatusText(this int statusCode)
    {
        if (Enum.IsDefined(typeof(HttpStatusCode), statusCode))
        {
            var name = Enum.GetName(typeof(HttpStatusCode), statusCode);
            var formatted = name?.Replace('_', ' ') ?? "Unknown status";
            return $"[{statusCode} {formatted}]";
        }

        return "[Unknown status]";
    }
}