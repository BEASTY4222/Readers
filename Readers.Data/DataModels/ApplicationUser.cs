using Microsoft.AspNetCore.Identity;

namespace Readers.Data.DataModels
{
    /// <summary>
    /// Represents an application user entity, extending ASP.NET Core Identity's IdentityUser.
    /// Contains additional application-specific user information and relationships to user-generated content (comments and likes).
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Gets or sets the user-typed display name.
        /// </summary>
        /// <remarks>
        /// This is a custom field used for user profiles and display purposes throughout the application.
        /// Distinct from the username provided by IdentityUser.
        /// </remarks>
        public string UserTypedName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the collection of comments posted by this user on books.
        /// </summary>
        /// <remarks>
        /// Virtual collection for lazy loading or eager loading with Include().
        /// Initialized to an empty list by default.
        /// Populated when a user creates or edits comments on books.
        /// </remarks>
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        /// <summary>
        /// Gets or sets the collection of books liked by this user.
        /// </summary>
        /// <remarks>
        /// Virtual collection for lazy loading or eager loading with Include().
        /// Initialized to an empty list by default.
        /// Populated when a user likes a book via the ToggleLike action.
        /// </remarks>
        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}
