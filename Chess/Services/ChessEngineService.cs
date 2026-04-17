using Chess.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Chess.Services
{
    public class ChessEngineService : IChessEngineService
    {
        public bool IsMoveValid(PieceType type, int x1, int y1, int x2, int y2, string[,] board, bool isCapture)
        {
            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);

            return type switch
            {
                PieceType.Knight => (dx == 1 && dy == 2) || (dx == 2 && dy == 1),
                PieceType.Rook => x1 == x2 || y1 == y2,
                PieceType.Bishop => dx == dy,
                PieceType.Queen => (x1 == x2 || y1 == y2) || (dx == dy),
                PieceType.King => dx <= 1 && dy <= 1,
                PieceType.Pawn => IsPawnMoveValid(x1, y1, x2, y2, isCapture),
                _ => false
            };
        }

        private bool IsPawnMoveValid(int x1, int y1, int x2, int y2, bool isCapture)
        {
            int dx = Math.Abs(x2 - x1);
            int dy = y2 - y1;
            if (!isCapture) return dx == 0 && (dy == 1 || (y1 == 1 && dy == 2));
            return dx == 1 && Math.Abs(dy) == 1;
        }
    }
}
