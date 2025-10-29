using Microsoft.EntityFrameworkCore;
using MyPersonalLibrary.Server.Endpoints;
using MyPersonalLibrary.Server.Exceptions;
using MyPersonalLibrary.Server.Models.Context;
using MyPersonalLibrary.Server.Profiles;
using MyPersonalLibrary.Server.Repositories;
using MyPersonalLibrary.Server.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("MyPersonalLibraryDB");
var allowedOrigins = builder.Configuration.GetValue<string>("AllowedOrigins");

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddProblemDetails();    
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddDbContext<MyPersonalLibraryContext>(options =>
{
    options.UseSqlServer(connectionString,
         sqlServerOptionsAction =>
         {
             // Abilita la resilienza (Retry logic) specifica per SQL Server
             sqlServerOptionsAction.EnableRetryOnFailure(
                 maxRetryCount: 5,
                 maxRetryDelay: TimeSpan.FromSeconds(30),
                 errorNumbersToAdd: null
             );
         }
    );
});
builder.Services.AddOpenApi();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddAutoMapper(typeof(BookProfile));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueClient", policy =>
    {
        policy.WithOrigins(allowedOrigins!)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();
app.UseCors("AllowVueClient");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1");
    });
}

app.MapBookEndpoints();
app.UseExceptionHandler();
app.Run();