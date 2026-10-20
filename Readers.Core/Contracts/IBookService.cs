using Microsoft.AspNetCore.Mvc;
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
        /// Retrieves all books from the database with related entities (author, likes, comments).
        /// </summary>
        /// <returns>An enumerable collection of all books ordered by author name and year published.</returns>
        /// <remarks>Results are limited to MaxBooksToDisplay as per application configuration.</remarks>
        Task<IEnumerable<Book>> GetAllBooks();

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
        Task<int> GetAuthorBookCount(int id);

        /// <summary>
        /// Retrieves a specific book by its identifier, including all related data.
        /// </summary>
        /// <param name="id">The unique identifier of the book to retrieve.</param>
        /// <returns>The book object if found; null if the book does not exist.</returns>
        /// <remarks>Includes related entities: author, likes, and comments with user information.</remarks>
        Task<Book?> GetByIdAsync(int id);

        /// <summary>
        /// Searches for books based on optional title, author, and genre criteria.
        /// </summary>
        /// <param name="title">Optional book title search term (case-insensitive, ignores whitespace).</param>
        /// <param name="author">Optional author name search term (case-insensitive, ignores whitespace).</param>
        /// <param name="genre">Optional exact genre match.</param>
        /// <returns>An enumerable collection of matching books ordered by author name.</returns>
        /// <remarks>
        /// If no criteria are provided, returns all available books. 
        /// Results are limited to MaxBooksToDisplay as per application configuration.
        /// </remarks>
        Task<IEnumerable<Book>> SearchAsync(string? title, string? author, string? genre);

        /// <summary>
        /// Toggles the like status for a book by a specific user.
        /// </summary>
        /// <param name="bookId">The unique identifier of the book.</param>
        /// <param name="userId">The unique identifier of the user toggling the like.</param>
        /// <returns>True if the book is now liked; false if the like was removed.</returns>
        /// <remarks>If the user has already liked the book, the like is removed; otherwise, a new like is added.</remarks>
        Task<bool> ToggleLikeAsync(int bookId, string userId);

        /// <summary>
        /// Checks whether a specific user has liked a particular book.
        /// </summary>
        /// <param name="bookId">The unique identifier of the book.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>True if the user has liked the book; false otherwise.</returns>
        Task<bool> UserHasLikedAsync(int bookId, string userId);
    }
}
