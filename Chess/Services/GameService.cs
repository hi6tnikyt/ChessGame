using Chess.Data;
using Chess.Models;
using Chess.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Chess.Services
{
    public class GameService : IGameService
    {
        private readonly ChessDbContext _context;
        private readonly IChessEngineService _engineService;

        public GameService(ChessDbContext context, IChessEngineService engineService)
        {
            _context = context;
            _engineService = engineService;
        }

        public async Task<Game> CreateGameAsync(int whitePlayerId, int blackPlayerId)
        {
            var game = new Game
            {
                WhitePlayerId = whitePlayerId,
                BlackPlayerId = blackPlayerId,
                BoardFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR",
                CurrentTurn = "White",
                CreatedAt = DateTime.UtcNow, 
                Status = "Active"          
            };

            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return game;
        }

        public async Task<bool> MakeMoveAsync(int gameId, int x1, int y1, int x2, int y2, int playerId)
        {
            var game = await _context.Games.FindAsync(gameId);

            if (game == null || game.Status != "Active") return false;

            int expectedPlayerId = (game.CurrentTurn == "White") ? game.WhitePlayerId : game.BlackPlayerId;
            if (playerId != expectedPlayerId) return false;

            string[,] currentBoard = ParseBoard(game.BoardFen);
            var pieceType = GetPieceTypeAt(currentBoard, x1, y1);

            bool isCapture = !string.IsNullOrEmpty(currentBoard[x2, y2]);

            bool isValid = _engineService.IsMoveValid(pieceType, x1, y1, x2, y2, currentBoard, isCapture);

            if (isValid)
            {
                var move = new Move
                {
                    GameId = gameId,
                    MoveText = $"{x1},{y1}-{x2},{y2}",
                    MoveNumber = game.Moves.Count + 1,
                    MoveTime = DateTime.UtcNow 
                };
                _context.Moves.Add(move);

                game.BoardFen = UpdateBoard(currentBoard, x1, y1, x2, y2);
                game.CurrentTurn = (game.CurrentTurn == "White") ? "Black" : "White";

                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<List<Game>> GetPlayerGamesAsync(int userId)
        {
            return await _context.Games
                .Where(g => g.WhitePlayerId == userId || g.BlackPlayerId == userId)
                .ToListAsync();
        }

        public async Task<Game?> GetGameByIdAsync(int gameId)
        {
            return await _context.Games
                .Include(g => g.Moves)
                .FirstOrDefaultAsync(g => g.Id == gameId);
        }

        private string[,] ParseBoard(string fen) { return new string[8, 8]; }
        private PieceType GetPieceTypeAt(string[,] board, int x, int y) { return PieceType.Pawn; }
        private string UpdateBoard(string[,] board, int x1, int y1, int x2, int y2) { return "updated_fen"; }
    }
}