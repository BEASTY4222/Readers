using Microsoft.AspNetCore.Identity;

namespace Readers.Data.DataModels
{
    public class ApplicationUser : IdentityUser
    {
        public string Username { get; set; } = null!;
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}
