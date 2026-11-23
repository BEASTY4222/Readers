using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Readers.Common;
using Readers.Core.Contracts;
using Readers.Data;
using Readers.Data.DataModels;

namespace Readers.Core.Services
{
    public class ShelfService(ApplicationDbContext context) : IShelfService
    {
        public async Task UpdateShelfStatusAsync(string userId, Guid bookId, ShelfStatus status)
        {
            // 1. Look for a shelf entry for THIS SPECIFIC BOOK
            UserShelf? existingShelf = await context.UserShelves
                .FirstOrDefaultAsync(us => us.UserId == userId && us.BookId == bookId);

            if (existingShelf != null)
            {
                // Update the status of the existing entry
                existingShelf.status = status;
            }
            else
            {
                // Create a brand new entry for this book
                context.UserShelves.Add(new UserShelf
                {
                    UserId = userId,
                    BookId = bookId,
                    status = status
                });
            }

            await context.SaveChangesAsync();
        }

        public async Task<ShelfStatus?> GetShelfAsync(string userId, Guid bookId)
        {
            // Fetch the UserShelf entry for the given user and book
            var userShelf = await context.UserShelves.FirstOrDefaultAsync(us => us.UserId == userId && us.BookId == bookId);
            return userShelf?.status;
        }
    }
}
