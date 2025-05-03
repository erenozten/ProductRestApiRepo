using FluentValidation;
using FluentValidation.AspNetCore;
using ProductRestApi.Validators;

namespace ProductRestApi.Configurations;

public static class FluentValidationConfiguration
{
    public static void AddFluentValidationServices(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();

        // Assembly taraması
        services.AddValidatorsFromAssemblyContaining<Program>();
        services.AddValidatorsFromAssemblyContaining<ProductPostRequestDtoValidator>();
    }
}