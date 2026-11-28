namespace Readers.Web.ViewModels.AuthorViewModels
{
    public class AuthorListViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Country { get; set; }
        public string? ImageUrl { get; set; }
        public int BookCount { get; set; }
    }
}