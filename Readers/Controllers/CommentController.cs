using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Readers.Common;
using Readers.Core.Contracts;
using Readers.Data.DataModels;

namespace Readers.Web.Controllers
{
    public class CommentController(ICommentService commentService, UserManager<ApplicationUser> userManager) : Controller
    {
        [Authorize]
        [HttpPost]
        [Route("/Comment/Create")]
        public async Task<IActionResult> Create([FromForm] int id, [FromForm] string text)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                ModelState.AddModelError("text", "Comment text cannot be empty.");
                return RedirectToAction($"Details", "Books", new { id = id });
            }

            if (text.Length > EntityDataLimits.CommentContentMaxLength)
            {
                ModelState.AddModelError("text", "Comment text cannot exceed 500 characters.");
                return RedirectToAction($"Details", "Books", new { id = id });
            }

            if (text.Length < EntityDataLimits.CommentContentMinLenght)
            {
                ModelState.AddModelError("text", "Comment text must be at least 5 characters long.");
                return RedirectToAction($"Details", "Books", new { id = id });
            }

            // Slim chance of this happeing but still better to be safe than sorry
            string userId = userManager.GetUserId(User) ?? throw new InvalidOperationException("User ID not found.");

            await commentService.CreateBookComment(id, text, userId);

            return RedirectToAction("Details", "Books", new { id = id });
        }
    }
}
