namespace Readers.Web.ViewModels
{
    public class BookIndexViewModel
    {
        public IReadOnlyList<BookViewModel> Books { get; init; } = Array.Empty<BookViewModel>();
        public IReadOnlyList<string> Genres { get; init; } = Array.Empty<string>();
        public SearchFormInputModel SearchFilters { get; init; } = new();
        public int CurrentPage { get; init; }
        public int TotalPages { get; init; }
        public int TotalBooks { get; init; }
    }
}
