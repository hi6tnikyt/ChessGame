using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chess.Controllers
{
    [Authorize]
    public class ChessboardController : Controller
    {
        public IActionResult Play()
        {
            return View();
        }
    }
}
