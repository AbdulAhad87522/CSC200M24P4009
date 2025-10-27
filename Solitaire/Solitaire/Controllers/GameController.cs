using Microsoft.AspNetCore.Mvc;

namespace Solitaire.Controllers
{
    public class GameController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
