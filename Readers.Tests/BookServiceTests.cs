using Microsoft.EntityFrameworkCore;
using Readers.Core.Services;
using Readers.Data;
using Readers.Data.DataModels;

namespace Readers.Tests;

public class BookServiceTests
{
    private static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private static Book CreateBook(string title, string authorName, string genre, int yearPublished)
    {
        var author = new Author { Name = authorName };
        var publisher = new Publisher { Name = "Test Publisher" };
        return new Book
        {
            Title = title,
            AuthorId = author.Id,
            Author = author,
            PublisherId = publisher.Id,
            Publisher = publisher,
            Genre = genre,
            YearPublished = yearPublished,
            Pages = 100
        };
    }

    [Test]
    public async Task GetAllBooks_OrdersByAuthorThenPublicationYear()
    {
        await using var context = CreateContext();
        context.Books.AddRange(
            CreateBook("Later", "Author B", "Fantasy", 2020),
            CreateBook("Newer", "Author A", "Fiction", 2022),
            CreateBook("Older", "Author A", "Fiction", 2018));
        await context.SaveChangesAsync();

        var books = (await new BookService(context).GetAllBooks()).ToList();

        Assert.That(books.Select(book => book.Title), Is.EqualTo(new[] { "Older", "Newer", "Later" }));
    }

    [Test]
    public async Task GetGenres_ReturnsDistinctGenres()
    {
        await using var context = CreateContext();
        context.Books.AddRange(
            CreateBook("One", "Author A", "Fiction", 2000),
            CreateBook("Two", "Author B", "Fiction", 2001),
            CreateBook("Three", "Author C", "History", 2002));
        await context.SaveChangesAsync();

        var genres = (await new BookService(context).GetGenres()).ToList();

        Assert.That(genres, Is.EquivalentTo(new[] { "Fiction", "History" }));
    }

    [Test]
    public async Task SearchAsync_NormalizesTitleAndAuthorAndAppliesGenre()
    {
        await using var context = CreateContext();
        context.Books.AddRange(
            CreateBook("The Silver Shore", "Jane Writer", "Fantasy", 2000),
            CreateBook("The Silver Shore", "Jane Writer", "History", 2001),
            CreateBook("Other Book", "Jane Writer", "Fantasy", 2002));
        await context.SaveChangesAsync();

        var matches = (await new BookService(context).SearchAsync("silvershore", "janewriter", "Fantasy")).ToList();

        Assert.That(matches, Has.Count.EqualTo(1));
        Assert.That(matches[0].Title, Is.EqualTo("The Silver Shore"));
        Assert.That(matches[0].Genre, Is.EqualTo("Fantasy"));
    }

    [Test]
    public async Task GetAuthorBookCount_ReturnsCountForRequestedAuthor()
    {
        await using var context = CreateContext();
        var author = new Author { Name = "Author A" };
        context.Books.AddRange(
            CreateBookForAuthor("One", author, 2000),
            CreateBookForAuthor("Two", author, 2001),
            CreateBook("Three", "Author B", "Fiction", 2002));
        await context.SaveChangesAsync();

        var count = await new BookService(context).GetAuthorBookCount(author.Id);

        Assert.That(count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetByIdAsync_ReturnsBookWithAuthor()
    {
        await using var context = CreateContext();
        var book = CreateBook("Target", "Author A", "Fiction", 2000);
        context.Books.Add(book);
        await context.SaveChangesAsync();

        var result = await new BookService(context).GetByIdAsync(book.Id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Title, Is.EqualTo("Target"));
        Assert.That(result.Author.Name, Is.EqualTo("Author A"));
        Assert.That(await new BookService(context).GetByIdAsync(Guid.NewGuid()), Is.Null);
    }

    [Test]
    public async Task ToggleLikeAsync_AddsThenRemovesLike()
    {
        await using var context = CreateContext();
        var book = CreateBook("Target", "Author A", "Fiction", 2000);
        context.Books.Add(book);
        await context.SaveChangesAsync();
        var service = new BookService(context);

        var liked = await service.ToggleLikeAsync(book.Id, "user-1");
        var hasLiked = await service.UserHasLikedAsync(book.Id, "user-1");
        var unliked = await service.ToggleLikeAsync(book.Id, "user-1");

        Assert.That(liked, Is.True);
        Assert.That(hasLiked, Is.True);
        Assert.That(unliked, Is.False);
        Assert.That(await service.UserHasLikedAsync(book.Id, "user-1"), Is.False);
    }

    private static Book CreateBookForAuthor(string title, Author author, int yearPublished)
    {
        var publisher = new Publisher { Name = "Test Publisher" };
        return new Book
        {
            Title = title,
            AuthorId = author.Id,
            Author = author,
            PublisherId = publisher.Id,
            Publisher = publisher,
            Genre = "Fiction",
            YearPublished = yearPublished,
            Pages = 100
        };
    }
}
