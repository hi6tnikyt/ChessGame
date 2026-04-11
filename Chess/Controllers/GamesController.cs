using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Chess.Data;
using Chess.Models;

namespace Chess.Controllers
{
    public class GamesController : Controller
    {
        private readonly ChessDbContext DbContext;

        public GamesController(ChessDbContext context)
        {
           this.DbContext = context;
        }

        public async Task<IActionResult> Index()
        {
            var chessDbContext = DbContext.Games.Include(g => g.BlackPlayer).Include(g => g.WhitePlayer);
            return View(await chessDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await DbContext.Games
                .Include(g => g.BlackPlayer)
                .Include(g => g.WhitePlayer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        public IActionResult Create()
        {
            ViewData["BlackPlayerId"] = new SelectList(DbContext.Players, "Id", "Id");
            ViewData["WhitePlayerId"] = new SelectList(DbContext.Players, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WhitePlayerId,BlackPlayerId,Status,Result,CreatedAt")] Game game)
        {
            if (ModelState.IsValid)
            {
                DbContext.Add(game);
                await DbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlackPlayerId"] = new SelectList(DbContext.Players, "Id", "Id", game.BlackPlayerId);
            ViewData["WhitePlayerId"] = new SelectList(DbContext.Players, "Id", "Id", game.WhitePlayerId);
            return View(game);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await DbContext.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            ViewData["BlackPlayerId"] = new SelectList(DbContext.Players, "Id", "Id", game.BlackPlayerId);
            ViewData["WhitePlayerId"] = new SelectList(DbContext.Players, "Id", "Id", game.WhitePlayerId);
            return View(game);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WhitePlayerId,BlackPlayerId,Status,Result,CreatedAt")] Game game)
        {
            if (id != game.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    DbContext.Update(game);
                    await DbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlackPlayerId"] = new SelectList(DbContext.Players, "Id", "Id", game.BlackPlayerId);
            ViewData["WhitePlayerId"] = new SelectList(DbContext.Players, "Id", "Id", game.WhitePlayerId);
            return View(game);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await DbContext.Games
                .Include(g => g.BlackPlayer)
                .Include(g => g.WhitePlayer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await DbContext.Games.FindAsync(id);
            if (game != null)
            {
                DbContext.Games.Remove(game);
            }

            await DbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return DbContext.Games.Any(e => e.Id == id);
        }
    }
}
