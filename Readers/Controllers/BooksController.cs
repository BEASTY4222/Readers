using Microsoft.AspNetCore.Mvc;

namespace Readers.Controllers
{
    public class BooksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
