using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Readers.Common;
using Readers.Core.Contracts;
using Readers.Data.DataModels;

namespace Readers.Web.Controllers
{
    /// <summary>
    /// Handles HTTP requests for comment management operations (create, edit, delete).
    /// All endpoints require user authentication and perform authorization checks to ensure
    /// users can only modify their own comments.
    /// </summary>
    public class CommentController(ICommentService commentService, UserManager<ApplicationUser> userManager) : Controller
    {
        /// <summary>
        /// Creates a new comment on a book.
        /// Validates comment content length and non-empty requirement before persisting to the database.
        /// </summary>
        /// <param name="id">The unique identifier of the book to add a comment to.</param>
        /// <param name="text">The comment text content provided by the user.</param>
        /// <returns>A redirect back to the book details page, or BadRequest if the book ID is invalid.</returns>
        /// <remarks>
        /// Requires user authentication. Validates:
        /// - Book ID is positive (id > 0)
        /// - Comment is not empty or whitespace-only
        /// - Comment length is between CommentContentMinLength and CommentContentMaxLength (5-500 characters)
        /// Model state errors are added and user is redirected to the book details page on validation failure.
        /// </remarks>
        [Authorize]
        [HttpPost]
        [Route("/Comment/Create")]
        public async Task<IActionResult> Create([FromForm] int id, [FromForm] string text)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                ModelState.AddModelError("text", "Comment text cannot be empty.");
                return RedirectToAction($"Details", "Books", new { id = id });
            }

            if (text.Length > EntityDataLimits.CommentContentMaxLength)
            {
                ModelState.AddModelError("text", "Comment text cannot exceed 500 characters.");
                return RedirectToAction($"Details", "Books", new { id = id });
            }

            if (text.Length < EntityDataLimits.CommentContentMinLenght)
            {
                ModelState.AddModelError("text", "Comment text must be at least 5 characters long.");
                return RedirectToAction($"Details", "Books", new { id = id });
            }

            // Retrieve the current user's ID; throws if not found (rare edge case, but defensive programming)
            string userId = userManager.GetUserId(User) ?? throw new InvalidOperationException("User ID not found.");

            await commentService.CreateAsync(id, text, userId);

            return RedirectToAction("Details", "Books", new { id = id });
        }

        /// <summary>
        /// Deletes a comment from a book.
        /// Only the comment author can delete their own comments.
        /// </summary>
        /// <param name="commentId">The unique identifier of the comment to delete.</param>
        /// <param name="bookId">The unique identifier of the book containing the comment (used for redirect).</param>
        /// <returns>A redirect back to the book details page, Forbid if the user is not the comment author, or Challenge if not authenticated.</returns>
        /// <remarks>
        /// Requires user authentication and CSRF protection via [ValidateAntiForgeryToken].
        /// Authorization is enforced by the CommentService, which verifies the user owns the comment.
        /// </remarks>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Comment/Delete")]
        public async Task<IActionResult> Delete(int commentId, int bookId)
        {
            var userId = userManager.GetUserId(User);
            if (userId == null) return Challenge();

            // Attempt to delete; returns false if user is not the comment author
            var success = await commentService.DeleteAsync(commentId, userId);
            if (!success) return Forbid();

            return RedirectToAction("Details", "Books", new { id = bookId });
        }

        /// <summary>
        /// Updates the content of an existing comment.
        /// Only the comment author can edit their own comments.
        /// </summary>
        /// <param name="commentId">The unique identifier of the comment to edit.</param>
        /// <param name="bookId">The unique identifier of the book containing the comment (used for redirect).</param>
        /// <param name="content">The new comment text content.</param>
        /// <returns>A redirect back to the book details page, Forbid if the user is not the comment author, or Challenge if not authenticated.</returns>
        /// <remarks>
        /// Requires user authentication and CSRF protection via [ValidateAntiForgeryToken].
        /// Validates that content is not empty and is between CommentContentMinLength and CommentContentMaxLength.
        /// Authorization is enforced by the CommentService, which verifies the user owns the comment.
        /// </remarks>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Comment/Edit")]
        public async Task<IActionResult> Edit(int commentId, int bookId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                ModelState.AddModelError("content", "Comment text cannot be empty.");
                return RedirectToAction("Details", "Books", new { id = bookId });
            }

            // Validate comment length is within acceptable bounds
            if (content.Length < EntityDataLimits.CommentContentMinLenght ||
                content.Length > EntityDataLimits.CommentContentMaxLength)
            {
                ModelState.AddModelError("content", "Comment length is invalid.");
                return RedirectToAction("Details", "Books", new { id = bookId });
            }

            var userId = userManager.GetUserId(User);
            if (userId == null) return Challenge();

            // Attempt to edit; returns false if user is not the comment author
            var success = await commentService.EditAsync(commentId, userId, content);
            if (!success) return Forbid();

            return RedirectToAction("Details", "Books", new { id = bookId });
        }
    }
}
