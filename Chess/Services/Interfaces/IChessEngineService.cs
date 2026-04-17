   using Chess.Models;
namespace Chess.Services.Interfaces
{
    public interface IChessEngineService
    {
        bool IsMoveValid(PieceType type, int x1, int y1, int x2, int y2, string[,] board, bool isCapture);
    }
}
