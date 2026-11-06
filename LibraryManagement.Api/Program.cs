using LibraryManagement.Api.DataAccess;
using LibraryManagement.Api.Endpoints;
using LibraryManagement.Api.Middleware;
using Scalar.AspNetCore;

namespace LibraryManagement.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder
            .AddApiServices()
            .AddApplicationServices()
            .AddDatabase()
            .AddAuthenticationServices()
            .AddErrorHandling();

        var app = builder.Build();

        app.UseExceptionHandler();
        app.UseHttpsRedirection();
        app.UseMiddleware<RequestTimingMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
            await DataSeeder.SeedAsync(app.Services);
        }

        app.MapCategoryEndpoints();
        app.MapControllers();

        app.Run();
    }
}
