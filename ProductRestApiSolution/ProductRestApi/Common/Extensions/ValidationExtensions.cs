using FluentValidation.Results;

namespace ProductRestApi.Common.Extensions;

public static class ValidationExtensions
{
    public static Dictionary<string, string[]> ToValidationDictionary(this List<ValidationFailure> failures)
    {
        return failures
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ErrorMessage).ToArray()
            );
    }
}
