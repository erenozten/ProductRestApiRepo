using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using ProductRestApi.Data;
using ProductRestApi.Interfaces.Repositories;
using ProductRestApi.Services;
using ProductRestApi.SwaggerExamples.Product;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using ProductRestApi.Middlewares;
using ProductRestApi.Validators;

var builder = WebApplication.CreateBuilder(args);

//  Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug() // 

    .MinimumLevel.Override("Microsoft", LogEventLevel.Error) 
    .MinimumLevel.Override("System", LogEventLevel.Error) 
    .MinimumLevel.Override("WeatherForecastApi.Middlewares", LogEventLevel.Information) // ← Kendi projen için alt limit

    .Enrich.FromLogContext()
    .WriteTo.Console(new RenderedCompactJsonFormatter()) 
    .WriteTo.File(new RenderedCompactJsonFormatter(), "Logs/log.json", rollingInterval: RollingInterval.Day)
    .WriteTo.Seq("http://localhost:5341", bufferBaseFilename: "Logs/seq-buffer", period: TimeSpan.FromSeconds(1)) // 🔥// ✅ Seq log sunucusuna gönderim. Linkten UI'a ulaşabilirsin.
    .WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
    {
        AutoRegisterTemplate = true,
        IndexFormat = "logstash-{0:yyyy.MM.dd}"
    })

    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation(); 
builder.Services.AddFluentValidationClientsideAdapters(); 
builder.Services
    .AddValidatorsFromAssemblyContaining<ProductPostRequestDtoValidator>();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Swagger

// Swagger JSON
builder.Services.AddEndpointsApiExplorer();
// Swagger UI
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations(); 
    c.ExampleFilters(); 
});
builder.Services.AddSwaggerExamplesFromAssemblyOf<SwaggerGetProduct200Response>();

// builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi("v1");

builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductApi V1");
        c.RoutePrefix = "swagger"; // → https://localhost:5001/swagger/index.html 
    });
}

app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();