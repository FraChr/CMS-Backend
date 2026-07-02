using Api.Config;
using Api.Dtos;
using Api.Interfaces;
using Api.Model;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddDbContext<Context>(options => 
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddScoped<ICertificateService, CertificateService>();
        builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();
        builder.Services.Configure<FileStorageOptions>(builder.Configuration.GetSection("FileOptions"));

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();


        app.MapGet("/certificates", async (
            [AsParameters] CertificateFilter filter,
            ICertificateService service) =>
        {
            var items = await service.GetCertificatesAsync(filter);
            return Results.Ok(items);
        });

        app.MapGet("/certificates/{id}/file", async (
            int id,
            ICertificateService service) =>
        {
            
            var result = await service.GetCertificateFileAsync(id);
            
            if(result == null)
                return Results.NotFound();
            
            return Results.File(result.Stream, result.ContentType, result.FileName);
            
        });

        app.MapPost("/certificate/upload", async (
            [FromForm] CreateCertificateRequest request,
            IFormFile file,
            ICertificateService service) =>
        {
            var certificate = new Certificate
            {
                Type = request.Type,
                Number = request.Number,
                NotifiedBody = request.NotifiedBody,
                IssueDate = request.IssueDate,
                ExpiryDate =  request.ExpirationDate
            };
            
            var created = await service.UploadCertificate(certificate, file);
            return Results.Created($"/certificates", created);
        }).DisableAntiforgery();
        
        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<Context>();
            db.Database.Migrate();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.Run();
    }
}