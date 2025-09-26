using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyPersonalLibrary.Server.Models;
using MyPersonalLibrary.Server.Models.Context;
using MyPersonalLibrary.Server.Models.DTOs;

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
