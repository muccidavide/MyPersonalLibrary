using MyPersonalLibrary.Server.Models;

namespace MyPersonalLibrary.Server.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book?> GetByIdAsync(int id);
        Task<Book> AddAsync(Book book);
        Task<bool> UpdateAsync(Book book);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Book>> GetPaginatedAsync(int pageNumber, int pageSize, string? title = null, string? author = null, string? year = null);
        Task<int> GetTotalAsync(string? title = null, string? author = null, string? year = null);
    }
}
