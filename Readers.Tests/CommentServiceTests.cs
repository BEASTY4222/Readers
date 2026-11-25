using Microsoft.EntityFrameworkCore;
using Readers.Core.DTOs.Input;
using Readers.Core.Services;
using Readers.Data;
using Readers.Data.DataModels;

namespace Readers.Tests;

public class CommentServiceTests
{
    private static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private static Book CreateBook(Guid id)
    {
        return new Book
        {
            Id = id,
            Title = "Test Book",
            AuthorId = Guid.NewGuid(),
            Genre = "Fiction",
            YearPublished = 2000,
            Pages = 100
        };
    }

    private static Comment CreateComment(Guid bookId, string userId, string content = "Original content")
    {
        return new Comment
        {
            BookId = bookId,
            UserId = userId,
            Content = content,
            CreatedAt = DateTime.UtcNow
        };
    }

    [Test]
    public async Task CreateAsync_PersistsCommentWithUtcTimestamp()
    {
        await using var context = CreateContext();
        var bookId = Guid.NewGuid();
        context.Books.Add(CreateBook(bookId));
        await context.SaveChangesAsync();
        var beforeCreate = DateTime.UtcNow;

        await new CommentService(context).CreateAsync(new CreateCommentDTO
        {
            BookId = bookId,
            UserId = "user-1",
            Content = "A useful comment"
        });

        var comment = await context.Comments.SingleAsync();
        Assert.That(comment.BookId, Is.EqualTo(bookId));
        Assert.That(comment.UserId, Is.EqualTo("user-1"));
        Assert.That(comment.Content, Is.EqualTo("A useful comment"));
        Assert.That(comment.CreatedAt.Kind, Is.EqualTo(DateTimeKind.Utc));
        Assert.That(comment.CreatedAt, Is.GreaterThanOrEqualTo(beforeCreate));
    }

    [Test]
    public void CreateAsync_ThrowsWhenBookDoesNotExist()
    {
        using var context = CreateContext();
        var service = new CommentService(context);

        Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(new CreateCommentDTO
        {
            BookId = Guid.NewGuid(),
            UserId = "user-1",
            Content = "A useful comment"
        }));
    }

    [Test]
    public async Task EditAsync_TrimsContentForCommentOwner()
    {
        await using var context = CreateContext();
        var comment = CreateComment(Guid.NewGuid(), "user-1");
        context.Comments.Add(comment);
        await context.SaveChangesAsync();

        var result = await new CommentService(context).EditAsync(new EditCommentDTO
        {
            CommentId = comment.Id,
            UserId = "user-1",
            Content = "  Updated content  "
        });

        Assert.That(result, Is.True);
        Assert.That(comment.Content, Is.EqualTo("Updated content"));
    }

    [Test]
    public async Task EditAsync_ReturnsFalseForAnotherUsersCommentOrMissingComment()
    {
        await using var context = CreateContext();
        var comment = CreateComment(Guid.NewGuid(), "owner");
        context.Comments.Add(comment);
        await context.SaveChangesAsync();
        var service = new CommentService(context);

        var unauthorizedResult = await service.EditAsync(new EditCommentDTO
        {
            CommentId = comment.Id,
            UserId = "another-user",
            Content = "Unauthorized edit"
        });
        var missingResult = await service.EditAsync(new EditCommentDTO
        {
            CommentId = Guid.NewGuid(),
            UserId = "owner",
            Content = "Missing comment"
        });

        Assert.That(unauthorizedResult, Is.False);
        Assert.That(missingResult, Is.False);
        Assert.That(comment.Content, Is.EqualTo("Original content"));
    }

    [Test]
    public async Task DeleteAsync_DeletesOnlyWhenRequestedByCommentOwner()
    {
        await using var context = CreateContext();
        var comment = CreateComment(Guid.NewGuid(), "owner");
        context.Comments.Add(comment);
        await context.SaveChangesAsync();
        var service = new CommentService(context);

        var unauthorizedResult = await service.DeleteAsync(new DeleteCommentDTO
        {
            CommentId = comment.Id,
            UserId = "another-user"
        });
        var remainingCount = await context.Comments.CountAsync();
        var ownerResult = await service.DeleteAsync(new DeleteCommentDTO
        {
            CommentId = comment.Id,
            UserId = "owner"
        });

        Assert.That(unauthorizedResult, Is.False);
        Assert.That(remainingCount, Is.EqualTo(1));
        Assert.That(ownerResult, Is.True);
        Assert.That(await context.Comments.AnyAsync(), Is.False);
    }
}
