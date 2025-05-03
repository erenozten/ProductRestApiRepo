namespace ProductRestApi.Configurations;

using Swashbuckle.AspNetCore.Filters;
using ProductRestApi.SwaggerExamples.Product;


public static class SwaggerConfiguration
{
    public static void AddSwaggerServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.ExampleFilters();
        });

        services.AddSwaggerExamplesFromAssemblyOf<SwaggerGetProduct200Response>();

        // Eğer OpenAPI ekliyorsan buraya taşı
        services.AddOpenApi("v1");
    }

    public static void UseSwaggerUiConfiguration(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductApi V1");
            c.RoutePrefix = "swagger";
        });
    }
}