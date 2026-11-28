namespace Readers.Web.ViewModels
{
    public class AuthorsIndexViewModel
    {
        public List<AuthorListViewModel> FeaturedAuthors { get; set; } = new();
        public List<AuthorListViewModel> AllAuthors { get; set; } = new();
    }
}