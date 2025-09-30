﻿using MyPersonalLibrary.Server.Models;
using MyPersonalLibrary.Server.Models.DTOs;
using MyPersonalLibrary.Server.Models.Utils;

namespace MyPersonalLibrary.Server.Services
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllBooksAsync();
        Task<PaginatedResult<BookDto>> GetPaginatedBooksAsync(int pageNumber = 1, int pageSize = 24);
        Task<BookDto?> GetBookByIdAsync(int id);
        Task<BookDto> AddBookAsync(BookDto bookDto);
        Task<bool> UpdateBookAsync(int id, BookDto bookDto);
        Task<bool> DeleteBookAsync(int id);
    }

}
