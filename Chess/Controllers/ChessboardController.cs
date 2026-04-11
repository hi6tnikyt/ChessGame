using Microsoft.AspNetCore.Mvc;

namespace Chess.Controllers
{
    public class ChessboardController : Controller
    {
        public IActionResult Play()
        {
            return View();
        }
    }
}
