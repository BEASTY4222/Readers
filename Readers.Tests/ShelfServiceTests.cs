using Microsoft.EntityFrameworkCore;
using Readers.Common;
using Readers.Core.Services;
using Readers.Data;
using Readers.Data.DataModels;

namespace Readers.Tests;

public class ShelfServiceTests
{
    private static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Test]
    public async Task UpdateShelfStatusAsync_CreatesEntryForUserAndBook()
    {
        await using var context = CreateContext();
        var service = new ShelfService(context);
        var bookId = Guid.NewGuid();

        await service.UpdateShelfStatusAsync("user-1", bookId, ShelfStatus.WantToRead);

        var shelf = await context.UserShelves.SingleAsync();
        Assert.That(shelf.UserId, Is.EqualTo("user-1"));
        Assert.That(shelf.BookId, Is.EqualTo(bookId));
        Assert.That(shelf.status, Is.EqualTo(ShelfStatus.WantToRead));
        Assert.That(await service.GetShelfAsync("user-1", bookId), Is.EqualTo(ShelfStatus.WantToRead));
    }

    [Test]
    public async Task UpdateShelfStatusAsync_UpdatesMatchingEntryWithoutChangingOtherUsers()
    {
        await using var context = CreateContext();
        var bookId = Guid.NewGuid();
        context.UserShelves.AddRange(
            new UserShelf { UserId = "user-1", BookId = bookId, status = ShelfStatus.WantToRead },
            new UserShelf { UserId = "user-2", BookId = bookId, status = ShelfStatus.Read });
        await context.SaveChangesAsync();
        var service = new ShelfService(context);

        await service.UpdateShelfStatusAsync("user-1", bookId, ShelfStatus.CurrentlyReading);

        Assert.That(await service.GetShelfAsync("user-1", bookId), Is.EqualTo(ShelfStatus.CurrentlyReading));
        Assert.That(await service.GetShelfAsync("user-2", bookId), Is.EqualTo(ShelfStatus.Read));
        Assert.That(await context.UserShelves.CountAsync(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetShelfAsync_ReturnsNullWhenEntryDoesNotExist()
    {
        await using var context = CreateContext();

        var status = await new ShelfService(context).GetShelfAsync("user-1", Guid.NewGuid());

        Assert.That(status, Is.Null);
    }
}
