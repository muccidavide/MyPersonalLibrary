using AutoMapper;
using MyPersonalLibrary.Server.Models.DTOs;
using MyPersonalLibrary.Server.Models.Utils;
using MyPersonalLibrary.Server.Repositories;

namespace MyPersonalLibrary.Server.Services
{
    public class BookService : IBookService
    {
        private IMapper Mapper { get; set; }
        private IBookRepository Repository { get; set; }
        public BookService(IBookRepository repo, IMapper mapper)
        {
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            Repository = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
        {
            var books = await Repository.GetAllAsync();
            return books is null ? new List<BookDto>() : Mapper.Map<IList<BookDto>>(books);
        }

        public async Task<PaginatedResult<BookDto>> GetPaginatedBooksAsync(int pageNumber = 1, int pageSize = 24, string? title = null, string? author = null, string? year = null)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 24;

            var totalItems = await Repository.GetTotalAsync(title, author, year);
            var books = await Repository.GetPaginatedAsync(pageNumber, pageSize, title, author, year);
            var bookDtos = Mapper.Map<List<BookDto>>(books);

            return PaginatedResult<BookDto>.Paginate(bookDtos, totalItems, pageNumber, pageSize);
        }


        public Task<BookDto?> GetBookByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than zero");

            return Repository.GetByIdAsync(id)
                .ContinueWith(task => task.Result is null
                    ? null
                    : Mapper.Map<BookDto>(task.Result));
        }

        public async Task<BookDto> AddBookAsync(BookDto bookDto)
        {
            ArgumentNullException.ThrowIfNull(bookDto);

            ValidateBook(bookDto);

            var addedBook = await Repository.AddAsync(Mapper.Map<Models.Book>(bookDto));
            return Mapper.Map<BookDto>(addedBook);
        }

        public async Task<bool> UpdateBookAsync(int id, BookDto bookDto)
        {
            ArgumentNullException.ThrowIfNull(bookDto);

            ValidateBook(bookDto);

            var existingBook = await Repository.GetByIdAsync(id);
            if (existingBook is null)
                return false;

            bookDto.Id = id;
            var bookToUpdate = Mapper.Map<Models.Book>(bookDto);
            return await Repository.UpdateAsync(bookToUpdate);
        }

        public async Task<bool> DeleteBookAsync(int id)
            => await Repository.GetByIdAsync(id) is not null
                && await Repository.DeleteAsync(id);

        private void ValidateBook(BookDto book)
        {
            if (string.IsNullOrWhiteSpace(book.Title))
                throw new ArgumentException("Title is required");

            if (string.IsNullOrWhiteSpace(book.Authors))
                throw new ArgumentException("Authors are required");

            if (book.OriginalPublicationYear is < 0 or > 2100)
                throw new ArgumentException("Invalid publication year");
        }
    }
}