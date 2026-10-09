using Microsoft.AspNetCore.Mvc;
using Readers.Web.ViewModels;
using System.Diagnostics;

namespace Readers.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            char separator = returnUrl.Contains('?') ? '&' : '?';
            string redirectUrl = $"{returnUrl}{separator}culture={culture}";
            return LocalRedirect(redirectUrl);
        }
    }
}
