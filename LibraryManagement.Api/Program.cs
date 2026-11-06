using FluentValidation;
using LibraryManagement.Api.DataAccess;
using LibraryManagement.Api.DTOs.Authors;
using LibraryManagement.Api.DTOs.Categories;
using LibraryManagement.Api.Endpoints;
using LibraryManagement.Api.Entities;
using LibraryManagement.Api.Filters;
using LibraryManagement.Api.Mapping;
using LibraryManagement.Api.Middleware;
using LibraryManagement.Api.Validators.Authors;
using LibraryManagement.Api.Validators.Categories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace LibraryManagement.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddOpenApi();
        builder.Services.AddControllers();
        builder.Services.AddRouting(options => options.LowercaseUrls = true);
        builder.Services.AddDbContext<LibraryDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("LibraryDb")));

        builder.Services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<LibraryDbContext>();

        builder.Services.AddProblemDetails(); // UseExceptionHandler needs it for returning a proper Problem response
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        builder.Services.AddScoped<LogActionFilter>();
        builder.Services.AddScoped<AuthorMapper>();
        builder.Services.AddScoped<CategoryMapper>();
        builder.Services.AddScoped<BookMapper>();

        builder.Services.AddScoped<IValidator<CreateAuthorDto>, CreateAuthorDtoValidator>();
        builder.Services.AddScoped<IValidator<CreateCategoryDto>, CreateCategoryDtoValidator>();
        builder.Services.AddScoped<IValidator<UpdateCategoryDto>, UpdateCategoryDtoValidator>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
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
