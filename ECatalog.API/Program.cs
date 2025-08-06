
using ECatalog.API.Middleware;
using ECatalog.Application;
using ECatalog.Application.Interfaces;
using ECatalog.Infrastructure.Persistence;
using ECatalog.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Prometheus;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace ECatalog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Set serilog
            var logPath = Path.Combine(AppContext.BaseDirectory, "Logs", "log-.txt");


            try
            {
                Log.Information("Starting Online E-Catalog up...");
                var builder = WebApplication.CreateBuilder(args);
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

                builder.Host.UseSerilog((context, services, configuration) =>
                {
                    configuration
                        .Enrich.FromLogContext()
                        .Enrich.WithMachineName()
                        .Enrich.WithThreadId()
                        .Enrich.WithProcessId()
                        .Enrich.WithProperty("Service", "CatalogService")
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Information) // <-- Important: Allow hosting logs
                        .MinimumLevel.Override("System", LogEventLevel.Warning)
                        .MinimumLevel.Information()
                        //.WriteTo.Console(new RenderedCompactJsonFormatter())
                        .WriteTo.Seq("http://localhost:5341")
                        .WriteTo.File(
                            new RenderedCompactJsonFormatter(),
                            path: Path.Combine(AppContext.BaseDirectory, "Logs", "log-.txt"),
                            rollingInterval: RollingInterval.Day,
                            retainedFileCountLimit: 7,
                            fileSizeLimitBytes: 10_000_000,
                            rollOnFileSizeLimit: true,
                            shared: true,
                            flushToDiskInterval: TimeSpan.FromSeconds(1)
                        );
                });

                // Add services to the container.
                builder.Services.AddDbContext<CatalogDbContext>(options =>
                {
                    options.UseNpgsql(connectionString);
                });


                builder.Services.AddControllers();

                // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
                builder.Services.AddOpenApi();

                builder.Services.AddScoped<ICatalogService, CatalogService>();
                builder.Services.AddScoped<ICatalogRepository, CatalogRepository>();

                builder.Services.AddHealthChecks()
                .AddNpgSql(
                    builder.Configuration.GetConnectionString("DefaultConnection")!,
                    name: "postgresql",
                    failureStatus: HealthStatus.Degraded
                );

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.MapOpenApi();
                }

                app.UseMiddleware<CorrelationIdMiddleware>();

                app.UseHttpsRedirection();

                app.UseHttpMetrics();

                app.UseAuthorization();
                app.UseSerilogRequestLogging();

                app.MapControllers();
                app.MapMetrics();

                app.MapGet("/", () => "Catalog API up and running.");
                app.MapHealthChecks("/health");

                using (var scope = app.Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
                    db.Database.Migrate();
                }

                // CI/CD local test - Aug 6

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }
    }
}
