namespace Readers.ViewModels
{
    public class BookIndexViewModel
    {
        public IReadOnlyList<BookViewModel> Books { get; init; } = Array.Empty<BookViewModel>();
        public IReadOnlyList<string> Genres { get; init; } = Array.Empty<string>();
    }
}
