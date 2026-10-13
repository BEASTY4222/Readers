using System;
using System.Collections.Generic;
using System.Text;

namespace Readers.Core.Contracts
{
    public interface ICommentService
    {
        Task CreateBookComment(int bookId, string text, string userId);
    }
}
