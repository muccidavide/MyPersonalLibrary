namespace MyPersonalLibrary.Server.Test
{
    using Xunit;
    using Microsoft.EntityFrameworkCore;
    using MyPersonalLibrary.Server.Models;
    using MyPersonalLibrary.Server.Models.Context;
    using MyPersonalLibrary.Server.Repositories;
    using System.Threading.Tasks;
    using System.Linq;

    public class TestBookRepository
    {
        private async Task<MyPersonalLibraryContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<MyPersonalLibraryContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var context =  new MyPersonalLibraryContext(options);
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            return context;
        }

        private Book GetSampleBook() => new Book
        {
            Title = "Sample Book",
            Authors = "Author Name",
            OriginalPublicationYear = 2020,
            LanguageCode = "en",
            AverageRating = 4.5,
            ImageUrl = "http://example.com/image.jpg",
            Description = "Sample description"
        };

        [Fact]
        public async Task AddAsync_ShouldAddBook()
        {
            var context = await GetDbContext();
            var repo = new BookRepository(context);

            var book = GetSampleBook();
            var result = await repo.AddAsync(book);

            Assert.NotNull(result);
            Assert.Equal("Sample Book", result.Title);
            Assert.Single(context.Books);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveBook()
        {
            var context = await GetDbContext();
            var repo = new BookRepository(context);

            var book = GetSampleBook();
            context.Books.Add(book);
            await context.SaveChangesAsync();
            context.Entry(book).State = EntityState.Detached;

            var deleted = await repo.DeleteAsync(book.Id!.Value);
            Assert.True(deleted);
            Assert.Empty(context.Books);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnBooks()
        {
            var context = await GetDbContext();
            var repo = new BookRepository(context);

            context.Books.AddRange(
                GetSampleBook(),
                GetSampleBook()
            );
            await context.SaveChangesAsync();

            var books = await repo.GetAllAsync();
            Assert.Equal(2, books.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectBook()
        {
            var context = await GetDbContext();
            var repo = new BookRepository(context);

            var book = GetSampleBook();
            context.Books.Add(book);
            await context.SaveChangesAsync();

            var result = await repo.GetByIdAsync((int)book.Id!);
            Assert.NotNull(result);
            Assert.Equal("Sample Book", result.Title);
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyBook()
        {
            var context = await GetDbContext();
            var repo = new BookRepository(context);

            var book = GetSampleBook();
            context.Books.Add(book);
            await context.SaveChangesAsync();

            book.Title = "Updated Title";
            var updated = await repo.UpdateAsync(book);

            Assert.True(updated);
            var result = await repo.GetByIdAsync(book.Id!.Value);
            Assert.Equal("Updated Title", result!.Title);
        }

        [Fact]
        public async Task GetTotalAsync_ShouldReturnCorrectCount()
        {
            var context = await GetDbContext();
            var repo = new BookRepository(context);

            context.Books.AddRange(
                GetSampleBook(),
                GetSampleBook(),
                GetSampleBook()
            );
            await context.SaveChangesAsync();

            var total = await repo.GetTotalAsync();
            Assert.Equal(3, total);
        }

        [Fact]
        public async Task GetPaginatedAsync_ShouldReturnCorrectPage()
        {
            var context = await GetDbContext();
            var repo = new BookRepository(context);

            for (int i = 1; i <= 50; i++)
            {
                context.Books.Add(new Book { Title = $"Book {i:D2}" });
            }
            await context.SaveChangesAsync();

            var page = await repo.GetPaginatedAsync(pageNumber: 2, pageSize: 10);
            Assert.Equal(10, page.Count());
            Assert.Contains(page, b => b.Title == "Book 11");
        }
    }

}
