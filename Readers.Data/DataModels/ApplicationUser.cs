using Microsoft.AspNetCore.Identity;

namespace Readers.Data.DataModels
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}
