using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Readers.Core.Contracts
{
    public interface ICommentService
    {
        Task CreateAsync(int bookId, string text, string userId);
        Task<bool> DeleteAsync(int commentId, string userId);
    }
}
