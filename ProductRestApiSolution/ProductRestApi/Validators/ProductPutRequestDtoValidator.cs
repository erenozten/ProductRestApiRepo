using FluentValidation;
using ProductRestApi.Common.Constants;
using ProductRestApi.DTOs.Product;

namespace ProductRestApi.Validators;

public class ProductPutRequestDtoValidator : AbstractValidator<ProductPutRequestDto>
{
    public ProductPutRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage(ConstMessages.Product_Name_NullError)
            .NotEmpty().WithMessage(ConstMessages.Product_Name_EmptyError)
            .MaximumLength(100).WithMessage(ConstMessages.Product_Name_MaxLengthError);
        
        RuleFor(x => x.About)
            .MaximumLength(2500).WithMessage(ConstMessages.Product_About_MaxLengthError);
    }
}
