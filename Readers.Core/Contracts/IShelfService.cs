using System;
using System.Collections.Generic;
using System.Text;
using Readers.Common;

namespace Readers.Core.Contracts
{
    public interface IShelfService
    {
        Task UpdateShelfStatusAsync(string userId, Guid bookId, ShelfStatus status);
        Task<ShelfStatus?> GetShelfAsync(string userId, Guid bookId);
    }
}
