using ProductRestApi.Common.Constants;
using ProductRestApi.Common.Responses;

namespace ProductRestApi.SwaggerExamples.Common;

public static class SwaggerErrorFactory
{
    public static GenericApiResponse<object> Create(int statusCode)
    {
        return statusCode switch
        {
            400 => GenericApiResponse<object>.Fail(null, StatusCodes.Status400BadRequest, ConstMessages.BAD_REQUEST_Description, ConstMessages.BAD_REQUEST),
            401 => GenericApiResponse<object>.Fail(null, StatusCodes.Status401Unauthorized, ConstMessages.UNAUTHORIZED_Description, ConstMessages.UNAUTHORIZED),
            403 => GenericApiResponse<object>.Fail(null, StatusCodes.Status403Forbidden, ConstMessages.FORBIDDEN_Description, ConstMessages.FORBIDDEN),
            404 => GenericApiResponse<object>.Fail(null, StatusCodes.Status404NotFound, ConstMessages.NotFound404Generic(5), ConstMessages.DATA_NOTFOUND),
            409 => GenericApiResponse<object>.Fail(null, StatusCodes.Status409Conflict, ConstMessages.DUPLICATE_PRODUCT_Description("bottle"), ConstMessages.DUPLICATE_PRODUCT),
            _ => GenericApiResponse<object>.Fail(null, StatusCodes.Status500InternalServerError, ConstMessages.INTERNAL_SERVER_ERROR_Description, ConstMessages.INTERNAL_SERVER_ERROR)
        };
    }
}

