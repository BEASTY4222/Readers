using Readers.Data;
using Readers.Data.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Readers.Core.Contracts
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooks();
        Task<IEnumerable<string>> GetGenres();
        Task<IEnumerable<Book>> GetSearchFields();
        Task<int> GetAuthorBookCount(int id);
        Task<Book?> GetByIdAsync(int id);
        Task<IEnumerable<Book>> SearchAsync(string? title, string? author, string? genre);
    }
}
