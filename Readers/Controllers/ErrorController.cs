using Microsoft.AspNetCore.Mvc;

namespace Readers.Web.Controllers
{
    // This route matches /Error/404, /Error/400, etc.
    [Route("Error/{statusCode:int}")]
    public class ErrorController : Controller
    {
        public IActionResult Index(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewData["Title"] = "Page Not Found";
                    ViewData["Header"] = "404 - Lost in the Stacks";
                    ViewData["Message"] = "We couldn't find the page you're looking for. It might have been moved, deleted, or never existed.";
                    ViewData["Icon"] = "🔍";
                    break;
                case 400:
                    ViewData["Title"] = "Bad Request";
                    ViewData["Header"] = "400 - Invalid Request";
                    ViewData["Message"] = "The request you sent was invalid or malformed. Please check your data and try again.";
                    ViewData["Icon"] = "⚠️";
                    break;
                case 403:
                    ViewData["Title"] = "Access Denied";
                    ViewData["Header"] = "403 - Forbidden";
                    ViewData["Message"] = "You don't have permission to access this page. Please log in with an account that has access.";
                    ViewData["Icon"] = "🔒";
                    break;
                default:
                    ViewData["Title"] = "Error";
                    ViewData["Header"] = "Something Went Wrong";
                    ViewData["Message"] = "An unexpected error occurred on our end. Please try again later.";
                    ViewData["Icon"] = "🚨";
                    break;
            }

            ViewData["StatusCode"] = statusCode;
            return View();
        }
    }
}