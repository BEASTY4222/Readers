using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Readers.Core.Contracts;
using Readers.Data;
using Readers.Data.DataModels;

namespace Readers.Core.Services
{
    public class CommentService(ApplicationDbContext context) : ICommentService
    {
        public async Task CreateAsync(int bookId, string text, string userId)
        {
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

        public async Task<bool> DeleteAsync(int commentId, string userId)
        {
            var comment = await context.Comments.FindAsync(commentId);
            if (comment == null || comment.UserId != userId)
                return false;

            context.Comments.Remove(comment);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditAsync(int commentId, string userId, string newContent)
        {
            var comment = await context.Comments.FindAsync(commentId);
            if (comment == null || comment.UserId != userId)
                return false;

            comment.Content = newContent.Trim();
            await context.SaveChangesAsync();
            return true;
        }
    }
}
