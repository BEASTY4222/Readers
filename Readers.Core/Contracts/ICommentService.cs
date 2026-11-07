using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Readers.Core.DTOs.Input;

namespace Readers.Core.Contracts
{
    /// <summary>
    /// Defines the contract for comment management operations.
    /// This interface abstracts the business logic for creating, editing, and deleting comments on books,
    /// supporting dependency injection and enabling testability.
    /// </summary>
    public interface ICommentService
    {
        /// <summary>
        /// Creates a new comment on a book by the authenticated user.
        /// </summary>
        /// <param name="bookId">The unique identifier of the book to comment on.</param>
        /// <param name="text">The comment text content.</param>
        /// <param name="userId">The unique identifier of the user creating the comment.</param>
        /// <returns>A completed task representing the asynchronous operation.</returns>
        /// <remarks>
        /// Throws InvalidOperationException if the book does not exist.
        /// The comment's CreatedAt timestamp is set to UTC Now at the time of creation.
        /// </remarks>
        Task CreateAsync(CreateCommentDTO commentDto);

        /// <summary>
        /// Deletes a comment if the requesting user is its author.
        /// </summary>
        /// <param name="commentId">The unique identifier of the comment to delete.</param>
        /// <param name="userId">The unique identifier of the user requesting deletion.</param>
        /// <returns>True if the comment was successfully deleted; false if the comment does not exist or the user is not the author.</returns>
        /// <remarks>Authorization is enforced: only the comment author can delete their own comments.</remarks>
        Task<bool> DeleteAsync(DeleteCommentDTO commentDto);

        /// <summary>
        /// Updates the content of a comment if the requesting user is its author.
        /// </summary>
        /// <param name="commentId">The unique identifier of the comment to edit.</param>
        /// <param name="userId">The unique identifier of the user requesting the edit.</param>
        /// <param name="newContent">The new comment text content.</param>
        /// <returns>True if the comment was successfully updated; false if the comment does not exist or the user is not the author.</returns>
        /// <remarks>
        /// Authorization is enforced: only the comment author can edit their own comments.
        /// The new content is trimmed before persisting.
        /// </remarks>
        Task<bool> EditAsync(EditCommentDTO commentDto);
    }
}
