using MyPersonalLibrary.Server.Models;
using MyPersonalLibrary.Server.Models.DTOs;
using MyPersonalLibrary.Server.Services;

namespace MyPersonalLibrary.Server.Endpoints
{
    public static class BookEndpoints
    {
        public static void MapBookEndpoints(this WebApplication app)
        {
            var booksGroup = app.MapGroup("/api/books")
                                .WithTags("Books API");

            booksGroup.MapGet("/", async (IBookService service) =>
                Results.Ok(await service.GetAllBooksAsync()));

            booksGroup.MapGet("/{id}", async (int id, IBookService service) =>
                await service.GetBookByIdAsync(id)
                    is BookDto book
                    ? Results.Ok(book)
                    : Results.NotFound());

            booksGroup.MapPost("/", async (BookDto bookDto, IBookService service) =>
            {

                var book = await service.AddBookAsync(bookDto);
                return Results.Created($"/api/books/{book.Id}", book);
            });

            booksGroup.MapPut("/{id}", async (int id, BookDto bookDto, IBookService service) =>
            {
                var success = await service.UpdateBookAsync(id, bookDto);
                return success ? Results.NoContent() : Results.NotFound();
            });

            booksGroup.MapDelete("/{id}", async (int id, IBookService service) =>
            {
                var success = await service.DeleteBookAsync(id);
                return success ? Results.NoContent() : Results.NotFound();
            });
        }
    }
}
