namespace ProductRestApi.Configurations;

using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Elasticsearch;

public static class LoggingConfiguration 
{
    public static void ConfigureLogging()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()

            .MinimumLevel.Override("Microsoft", LogEventLevel.Error)              // 'Microsoft' namespace'inin minimum log level ayarı
            .MinimumLevel.Override("System", LogEventLevel.Error)
            .MinimumLevel.Override("WeatherForecastApi.Middlewares", LogEventLevel.Information) 

            .Enrich.FromLogContext()
            .WriteTo.Console(new RenderedCompactJsonFormatter())
            .WriteTo.File(new RenderedCompactJsonFormatter(), "Logs/log.json", rollingInterval: RollingInterval.Day)
            .WriteTo.Seq("http://localhost:5341", bufferBaseFilename: "Logs/seq-buffer", period: TimeSpan.FromSeconds(1)) // Seq log sunucusuna gönderim. Linkten UI'a ulaşabilirsin.
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
            {
                AutoRegisterTemplate = true,
                IndexFormat = "logstash-{0:yyyy.MM.dd}"
            })
            .CreateLogger();
    }
}
