using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Readers.Core.Contracts;
using Readers.Data;
using Readers.Data.DataModels;

namespace Readers.Core.Services
{
    /// <summary>
    /// Implements comment management operations for books.
    /// Handles creation, editing, and deletion of user comments with authorization checks to ensure
    /// users can only modify their own comments.
    /// </summary>
    public class CommentService(ApplicationDbContext context) : ICommentService
    {
        /// <summary>
        /// Creates a new comment on a book and persists it to the database.
        /// </summary>
        /// <param name="bookId">The unique identifier of the book being commented on.</param>
        /// <param name="text">The comment text content.</param>
        /// <param name="userId">The unique identifier of the user creating the comment.</param>
        /// <exception cref="InvalidOperationException">Thrown if the book with the specified ID does not exist.</exception>
        /// <remarks>
        /// The comment's CreatedAt timestamp is automatically set to the current UTC time.
        /// Verifies that the book exists before creating the comment to maintain referential integrity.
        /// </remarks>
        public async Task CreateAsync(int bookId, string text, string userId)
        {
            // Verify the book exists before creating a comment
            var book = await context.Books.FindAsync(bookId)
            ?? throw new InvalidOperationException($"Book with id {bookId} not found.");

            context.Comments.Add(new Comment
            {
                BookId = bookId,
                Content = text,
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            });

            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a comment from the database if the requesting user is the comment's author.
        /// </summary>
        /// <param name="commentId">The unique identifier of the comment to delete.</param>
        /// <param name="userId">The unique identifier of the user requesting deletion (for authorization).</param>
        /// <returns>True if deletion was successful; false if the comment does not exist or the user is not the author.</returns>
        /// <remarks>
        /// Authorization is enforced: attempting to delete another user's comment will return false.
        /// This method uses a fail-silently pattern appropriate for API responses.
        /// </remarks>
        public async Task<bool> DeleteAsync(int commentId, string userId)
        {
            var comment = await context.Comments.FindAsync(commentId);

            // Check that the comment exists and the user is authorized to delete it
            if (comment == null || comment.UserId != userId)
                return false;

            context.Comments.Remove(comment);
            await context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Updates the content of a comment if the requesting user is the comment's author.
        /// </summary>
        /// <param name="commentId">The unique identifier of the comment to edit.</param>
        /// <param name="userId">The unique identifier of the user requesting the edit (for authorization).</param>
        /// <param name="newContent">The new comment text content.</param>
        /// <returns>True if the update was successful; false if the comment does not exist or the user is not the author.</returns>
        /// <remarks>
        /// Authorization is enforced: attempting to edit another user's comment will return false.
        /// The content is trimmed before persisting to remove leading/trailing whitespace.
        /// </remarks>
        public async Task<bool> EditAsync(int commentId, string userId, string newContent)
        {
            var comment = await context.Comments.FindAsync(commentId);

            // Check that the comment exists and the user is authorized to edit it
            if (comment == null || comment.UserId != userId)
                return false;

            comment.Content = newContent.Trim();
            await context.SaveChangesAsync();
            return true;
        }
    }
}
