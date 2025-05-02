using FluentValidation.Results;
using ProductRestApi.Common.Constants;
using ProductRestApi.Common.Extensions;
using ProductRestApi.Common.Responses;
using ProductRestApi.DTOs;

namespace ProductRestApi.Common.Helpers;

public class ApiResponseHelper
{
    public static GenericApiResponse<ListWithCountDto<T>> SuccessList<T>(List<T> items) =>
        GenericApiResponse<ListWithCountDto<T>>.SuccessList(items);

    public static GenericApiResponse<T> Success<T>(T data) =>
        GenericApiResponse<T>.Success(data, StatusCodes.Status200OK);

    public static GenericApiResponse<T> InternalError<T>() =>
        GenericApiResponse<T>.Fail(default, StatusCodes.Status500InternalServerError,
            ConstMessages.INTERNAL_SERVER_ERROR_Description, ConstMessages.INTERNAL_SERVER_ERROR);

    public static GenericApiResponse<T> BadRequest<T>() =>
        GenericApiResponse<T>.Fail(default, StatusCodes.Status400BadRequest,
            ConstMessages.BAD_REQUEST_Description, ConstMessages.BAD_REQUEST);

    public static GenericApiResponse<T> NotFound<T>(int id) =>
        GenericApiResponse<T>.Fail(default, StatusCodes.Status404NotFound,
            ConstMessages.NotFound404Generic(id), ConstMessages.DATA_NOTFOUND);

    public static GenericApiResponse<T> Duplicate<T>(string name) =>
        GenericApiResponse<T>.Fail(default, StatusCodes.Status409Conflict,
            ConstMessages.DUPLICATE_PRODUCT_Description(name), ConstMessages.DUPLICATE_PRODUCT);

    public static GenericApiResponse<T> InvalidParameter<T>() =>
        GenericApiResponse<T>.Fail(default, StatusCodes.Status400BadRequest,
            "Invalid parameters!!", ConstMessages.BAD_REQUEST);

    public static GenericApiResponse<T> Fail<T>(int statusCode, string message, string errorCode) =>
        GenericApiResponse<T>.Fail(default, statusCode, message, errorCode);

    public static GenericApiResponse<T> ValidationFail<T>(List<ValidationFailure> failures) =>
        GenericApiResponse<T>.Fail(
            data: default,
            statusCode: StatusCodes.Status400BadRequest,
            message: "Validation failed.",
            errorCode: ConstMessages.INVALID_INPUT,
            validationErrors: failures.ToValidationDictionary()
        );   
}