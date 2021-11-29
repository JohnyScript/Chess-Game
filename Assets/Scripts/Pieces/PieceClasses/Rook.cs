namespace Chess.Pieces
{
    using Managers;

    using System;

    public class Rook : Piece
    {
        public int[,] _LegalMoves = new int[4, 2]
        {
            { 0, 1},
            { 1, 0},
            { 0, -1},
            { -1, 0}
        };

        public int[,] GenerateLegalMoves()
        {
            return _LegalMoves;
        }
    }
}