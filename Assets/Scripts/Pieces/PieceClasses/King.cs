namespace Chess.Pieces
{
    using Managers;

    using System;

    public class King : Piece
    {
        public int[,] _LegalMoves = new int[8, 2]
        {
            { 0, 1},
            { 1, 1},
            { 1, 0},
            { 1, -1},
            { 0, -1},
            { -1, -1},
            { -1, 0},
            { -1, 1}
        };

        public int[,] GenerateLegalMoves()
        {
            return _LegalMoves;
        }
    }
}
