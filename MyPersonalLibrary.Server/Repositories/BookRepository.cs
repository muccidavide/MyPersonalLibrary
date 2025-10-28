using Microsoft.EntityFrameworkCore;
using MyPersonalLibrary.Server.Models;
using MyPersonalLibrary.Server.Models.Context;

namespace MyPersonalLibrary.Server.Repositories
{
    public class BookRepository : IBookRepository
    {
        private MyPersonalLibraryContext Context { get; }
        public BookRepository(MyPersonalLibraryContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Book> AddAsync(Book book)
        {
            ArgumentNullException.ThrowIfNull(book);

            Context.Books.Add(book);
            await Context.SaveChangesAsync();
            return book;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Context.Books.Remove(new Book { Id = id });
            return await Context.SaveChangesAsync() > 0;    
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
            => await Context.Books
                .Take(20)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Book?> GetByIdAsync(int id)
            => await Context.Books.FindAsync(id);

        public async Task<bool> UpdateAsync(Book book)
        {
            ArgumentNullException.ThrowIfNull(book);
            Context.Books.Update(book);
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<int> GetTotalAsync(string? title = null, string? author = null, string? year = null)
        {
            var query = Context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(b => b.Title != null && b.Title.Contains(title));

            if (!string.IsNullOrWhiteSpace(author))
                query = query.Where(b => b.Authors != null && b.Authors.Contains(author));

            if (!string.IsNullOrWhiteSpace(year) && int.TryParse(year, out int yearValue))
                query = query.Where(b => b.OriginalPublicationYear == yearValue);

            return await query.CountAsync();
        }

        public async Task<IEnumerable<Book>> GetPaginatedAsync(int pageNumber = 1, int pageSize = 24, string? title = null, string? author = null, string? year = null)
        {
            var query = Context.Books.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(b => b.Title != null && b.Title.Contains(title));

            if (!string.IsNullOrWhiteSpace(author))
                query = query.Where(b => b.Authors != null && b.Authors.Contains(author));

            if (!string.IsNullOrWhiteSpace(year) && int.TryParse(year, out int yearValue))
                query = query.Where(b => b.OriginalPublicationYear == yearValue);

            return await query
                .OrderBy(x => x.Title)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();
        }

    }
}
