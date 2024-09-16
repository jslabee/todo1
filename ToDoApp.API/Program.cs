using System.Reflection;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Services;
using ToDoApp.Domain.Interfaces;
using ToDoApp.Infrastructure.Data;
using ToDoApp.Infrastructure.Repositories;

namespace ToDoApp.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Konfiguracija log4net
        var assembly = Assembly.GetExecutingAssembly();
        var loggerRepository = LogManager.GetRepository(assembly);
        XmlConfigurator.Configure(loggerRepository, new FileInfo("log4net.config"));

        // Konfiguracija Entity Framework in SQL Server
        builder.Services.AddDbContext<ToDoContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.MigrationsAssembly("ToDoApp.Infrastructure")));

        // Registracija repozitorija in storitev
        builder.Services.AddScoped<IOpraviloRepository, OpraviloRepository>();
        builder.Services.AddScoped<IOpraviloService, OpraviloService>();

        // Dodaj storitve v kontejner
        builder.Services.AddControllers();

        // Dodaj Swagger/OpenAPI
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Konfiguracija obdelave HTTP zahtevkov
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Uporaba middleware za obdelavo napak
        app.UseExceptionHandler("/error");
        app.UseHsts();

        app.UseHttpsRedirection();
        app.UseAuthorization();

        // Poveže kontrolerje z URL potmi
        app.MapControllers(); 

        // Definicija prilagojene obdelave napak
        app.Map("/error", (HttpContext httpContext) =>
            {
                var exceptionHandlerPathFeature = httpContext.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature?.Error;

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Prišlo je do napake med obdelavo vaše zahteve.",
                    Detail = exception?.Message
                };

                if (exception is KeyNotFoundException)
                {
                    problemDetails.Status = StatusCodes.Status404NotFound;
                    problemDetails.Title = "Vir ni bil najden";
                }


                return Results.Problem(
                    problemDetails.Detail,
                    title: problemDetails.Title,
                    statusCode: problemDetails.Status
                );
            })
            .WithName("Error");

        app.Run();
    }
}