using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using MyPersonalLibrary.Server.Endpoints;
using MyPersonalLibrary.Server.Models.Context;
using MyPersonalLibrary.Server.Profiles;
using MyPersonalLibrary.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("MyPersonalLibraryDB");
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


}
);
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddAutoMapper(typeof(BookProfile));
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

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
// Configure the HTTP request pipeline.
// http://localhost:5153/swagger/index.html
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1");
    });
}

app.MapBookEndpoints();
app.Run();
