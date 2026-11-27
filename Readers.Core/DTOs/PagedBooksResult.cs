using Readers.Data.DataModels;

namespace Readers.Core.DTOs;

public sealed class PagedBooksResult
{
    public required IReadOnlyList<Book> Books { get; init; }
    public required int TotalCount { get; init; }
    public required int PageNumber { get; init; }
    public required int PageSize { get; init; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
