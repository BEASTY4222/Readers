namespace Readers.Web.ViewModels
{
    public class AuthorBookViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? CoverImagePath { get; set; }
        public int YearPublished { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
    }
}
