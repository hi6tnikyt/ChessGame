using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Chess.Data;
using Chess.Models;

namespace Chess.Controllers
{
    public class PlayersController : Controller
    {
        private readonly ChessDbContext DbContext;

        public PlayersController(ChessDbContext context)
        {
            DbContext = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await DbContext.Players.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Player? player = await DbContext.Players
                .FirstOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Rating,Wins,Losses,Draws,JoinedAt")] Player player)
        {
            if (ModelState.IsValid)
            {
                DbContext.Add(player);
                await DbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(player);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Player? player = await DbContext.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }
            return View(player);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Rating,Wins,Losses,Draws,JoinedAt")] Player player)
        {
            if (id != player.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    DbContext.Update(player);
                    await DbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.Id))
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
            return View(player);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Player? player = await DbContext.Players
                .FirstOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Player? player = await DbContext.Players.FindAsync(id);
            if (player != null)
            {
                DbContext.Players.Remove(player);
            }

            await DbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerExists(int id)
        {
            return DbContext.Players.Any(e => e.Id == id);
        }
    }
}
