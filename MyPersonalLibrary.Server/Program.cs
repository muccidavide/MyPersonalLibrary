using Microsoft.EntityFrameworkCore;
using MyPersonalLibrary.Server.Endpoints;
using MyPersonalLibrary.Server.Models.Context;
using MyPersonalLibrary.Server.Services;
using MyPersonalLibrary.Server.Profiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("MyPersonalLibraryDB");
builder.Services.AddDbContext<MyPersonalLibraryContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddAutoMapper(typeof(BookProfile));

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

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
