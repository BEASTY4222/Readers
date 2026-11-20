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
        public Task UpdateShelfStatusAsync(string userId, Guid bookId, ShelfStatus status)
        {
            // If we dont find an entry we create a new one, otherwise we update the existing entry
            if (context.UserShelves.Any(us => us.UserId == userId && us.BookId == bookId))
            {
                // Getting the existing UserShelf entry for the user and book
                UserShelf userShelf = context.UserShelves.First(us => us.UserId == userId && us.BookId == bookId);
                // Updating the status
                userShelf.status = status;
            }
            else
            {
                context.UserShelves.Add(new UserShelf
                {
                    UserId = userId,
                    BookId = bookId,
                    status = status
                });
            }
            
            return context.SaveChangesAsync();  
        }
    }
}
