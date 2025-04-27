using Swashbuckle.AspNetCore.Filters;
using ProductRestApi.Common.Constants;
using ProductRestApi.Common.Responses;

namespace ProductRestApi.SwaggerExamples.Product;

public class ProductGenericValidationErrorResponse : IExamplesProvider<GenericApiResponse<object>>
{
    public GenericApiResponse<object> GetExamples()
    {
        return GenericApiResponse<object>.Fail(
            data: null,
            statusCode: StatusCodes.Status400BadRequest,
            errorCode: ConstMessages.INVALID_INPUT,
            validationErrors: new Dictionary<string, string[]>
            {
                { "Name", new[] { ConstMessages.Product_Name_NullError, ConstMessages.Product_Name_EmptyError } },
                { "About", new[] { ConstMessages.Product_About_MaxLengthError } }
            }
        );
    }
}
