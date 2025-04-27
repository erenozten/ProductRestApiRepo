using System.Text.Json.Serialization;
using FluentValidation.Results;
using ProductRestApi.Common.Constants;
using ProductRestApi.Common.Extensions;
using ProductRestApi.DTOs;

namespace ProductRestApi.Common.Responses
{
    public class GenericApiResponse<T>
    {
        public bool IsSuccess { get; init; }
        public int StatusCode { get; init; }
        public string Message { get; init; } = string.Empty;
        public T? Data { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ErrorCode { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, string[]>? ValidationErrors { get; init; }

        public static GenericApiResponse<T> Success(T? data, int statusCode, string message = "")
        {
            return new GenericApiResponse<T>
            {
                Data = data,
                IsSuccess = true,
                StatusCode = statusCode,
                Message =
                    $"{statusCode.ToStatusText()} {ConstMessages.Success}{(string.IsNullOrWhiteSpace(message) ? "" : $" {message}")}",
            };
        }

        public static GenericApiResponse<T> Fail(
            T? data,
            int statusCode,
            string? message = "",
            string? errorCode = "",
            Dictionary<string, string[]>? validationErrors = null)
        {
            return new GenericApiResponse<T>
            {
                Data = data,
                IsSuccess = false,
                StatusCode = statusCode,
                Message =
                    $"{statusCode.ToStatusText()} {ConstMessages.Fail}{(string.IsNullOrWhiteSpace(message) ? "" : $" {message}")}",
                ErrorCode = errorCode,
                ValidationErrors = validationErrors
            };
        }

        public static GenericApiResponse<ListWithCountDto<T>> SuccessList<T>(List<T> items)
        {
            return GenericApiResponse<ListWithCountDto<T>>.Success(
                data: new ListWithCountDto<T>
                {
                    Items = items,
                    TotalCount = items.Count
                },
                statusCode: StatusCodes.Status200OK
            );
        }

    }
}