using Chess.Models;

namespace Chess.Services.Interfaces
{
    public interface IGameService
    {
        Task<Game> CreateGameAsync(int whitePlayerId, int blackPlayerId);
        Task<bool> MakeMoveAsync(int gameId, int x1, int y1, int x2, int y2, int playerId);
        Task<List<Game>> GetPlayerGamesAsync(int userId);
        Task<Game?> GetGameByIdAsync(int gameId);
    }
}