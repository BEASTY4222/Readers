namespace Readers.Web.ViewModels
{
    public class AuthorDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Country { get; set; }
        public string? ImageUrl { get; set; }
        public int TotalBooks { get; set; }

        public List<AuthorBookViewModel> Books { get; set; } = new();
    }

    
}