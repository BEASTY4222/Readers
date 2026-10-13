using Readers.Core.Contracts;
using Readers.Data;
using Readers.Data.DataModels;

namespace Readers.Core.Services
{
    public class CommentService(ApplicationDbContext context) : ICommentService
    {
        public async Task CreateBookComment(int bookId, string text, string userId)
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
    }
}
