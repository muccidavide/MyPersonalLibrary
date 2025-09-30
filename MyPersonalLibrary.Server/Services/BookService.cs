using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MyPersonalLibrary.Server.Models;
using MyPersonalLibrary.Server.Models.Context;
using MyPersonalLibrary.Server.Models.DTOs;
using MyPersonalLibrary.Server.Models.Utils;

namespace MyPersonalLibrary.Server.Services
{
    public class BookService : IBookService
    {
        private MyPersonalLibraryContext Context { get; set; }
        private IMapper Mapper { get; set; }
        public BookService(MyPersonalLibraryContext context, IMapper mapper)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
        {
            var books = await Context.Books.Take(20).ToListAsync();
            return Mapper.Map<List<BookDto>>(books);
        }

        public async Task<PaginatedResult<BookDto>> GetPaginatedBooksAsync(int pageNumber = 1, int pageSize = 24)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 24;

            var query = Context.Books.AsNoTracking();

            var totalItems = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.Title)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ProjectTo<BookDto>(Mapper.ConfigurationProvider)
                .ToListAsync();

            return PaginatedResult<BookDto>.Create(items, totalItems, pageNumber, pageSize);
        }


        public Task<BookDto?> GetBookByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BookDto> AddBookAsync(BookDto bookDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateBookAsync(int id, BookDto bookDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteBookAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
