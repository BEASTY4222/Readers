using Microsoft.AspNetCore.Mvc;
using Readers.Core.DTOs;
using Readers.Data;
using Readers.Data.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Readers.Core.Contracts
{
    /// <summary>
    /// Defines the contract for book data access and management operations.
    /// This interface abstracts the business logic layer, allowing for dependency injection
    /// and enabling testability by decoupling controllers from the concrete implementation.
    /// </summary>
    public interface IBookService
    {
        /// <summary>
        /// Retrieves a page of books matching optional title, author, and genre filters.
        /// </summary>
        /// <param name="title">Optional title search term.</param>
        /// <param name="author">Optional author search term.</param>
        /// <param name="genre">Optional exact genre match.</param>
        /// <param name="pageNumber">The one-based page number to retrieve.</param>
        /// <returns>The matching books for the requested page and pagination metadata.</returns>
        Task<PagedBooksResult> GetBooksPageAsync(string? title, string? author, string? genre, int pageNumber);

        /// <summary>
        /// Retrieves all distinct genres available in the database.
        /// </summary>
        /// <returns>An enumerable collection of unique genre strings.</returns>
        Task<IEnumerable<string>> GetGenres();

        /// <summary>
        /// Retrieves all books with their searchable fields loaded (author, likes, comments).
        /// </summary>
        /// <returns>An enumerable collection of all books for search operations.</returns>
        /// <remarks>This method is typically used to populate search indexes or retrieve data for filtering.</remarks>
        Task<IEnumerable<Book>> GetSearchFields();

        /// <summary>
        /// Counts the total number of books published by a specific author.
        /// </summary>
        /// <param name="id">The unique identifier of the author.</param>
        /// <returns>The total count of books written by the specified author.</returns>
        Task<int> GetAuthorBookCount(Guid id);

        /// <summary>
        /// Retrieves a specific book by its identifier, including all related data.
        /// </summary>
        /// <param name="id">The unique identifier of the book to retrieve.</param>
        /// <returns>The book object if found; null if the book does not exist.</returns>
        /// <remarks>Includes related entities: author, likes, and comments with user information.</remarks>
        Task<Book?> GetByIdAsync(Guid id);

        /// <summary>
        /// Toggles the like status for a book by a specific user.
        /// </summary>
        /// <param name="bookId">The unique identifier of the book.</param>
        /// <param name="userId">The unique identifier of the user toggling the like.</param>
        /// <returns>True if the book is now liked; false if the like was removed.</returns>
        /// <remarks>If the user has already liked the book, the like is removed; otherwise, a new like is added.</remarks>
        Task<bool> ToggleLikeAsync(Guid bookId, string userId);

        /// <summary>
        /// Checks whether a specific user has liked a particular book.
        /// </summary>
        /// <param name="bookId">The unique identifier of the book.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>True if the user has liked the book; false otherwise.</returns>
        Task<bool> UserHasLikedAsync(Guid bookId, string userId);
    }
}
