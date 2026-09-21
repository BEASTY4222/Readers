using Microsoft.AspNetCore.Mvc;

namespace Readers.Controllers
{
    public class BooksController : Controller
    {
        [Route("/Books")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
