using FluentValidation;
using FluentValidation.AspNetCore;

namespace ProductRestApi.Configurations;

public static class FluentValidationConfiguration
{
    public static void AddFluentValidationServices(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();

        services.AddValidatorsFromAssemblyContaining<Program>();                             // Assembly taraması: Bu satır sayesinde, Program.cs dosyasının bulunduğu assembly taranır. Eğer tüm validator sınıfların bu projede yer alıyorsa, bu satır genelde yeterlidir.
        // services.AddValidatorsFromAssemblyContaining<ProductPostRequestDtoValidator>();
    }
}