
using ECatalog.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace ECatalog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");



            // Add services to the container.
            builder.Services.AddDbContext<CatalogDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });


            builder.Services.AddControllers();


            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapGet("/", () => "Catalog API up and running.");

            app.Run();
        }
    }
}
