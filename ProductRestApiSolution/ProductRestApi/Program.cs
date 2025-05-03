using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductRestApi.Configurations;
using ProductRestApi.Data;
using ProductRestApi.Interfaces.Repositories;
using ProductRestApi.Services;
using Serilog;
using ProductRestApi.Middlewares;

var builder = WebApplication.CreateBuilder(args); // The WebApplication used to configure the HTTP pipeline, and routes.

LoggingConfiguration.ConfigureLogging();
builder.Host.UseSerilog();                        // bunu yazdığın anda, .NET’in default logging altyapısını override ediyorsun ve artık tüm ILogger<T> injection’ları Serilog üzerinden yapılmış oluyor.

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddControllers();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddFluentValidationServices();
builder.Services.AddSwaggerServices();
builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; }); // ASP.NET Core Web API'de ModelState geçerlilik kontrolünün otomatik yapılmasını devre dışı bırakır.

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUiConfiguration(); // UI'ı burada aktifleştiriyoruz
}

app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();