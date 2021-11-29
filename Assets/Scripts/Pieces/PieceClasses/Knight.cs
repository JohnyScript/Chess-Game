namespace Chess.Pieces
{
    using Managers;

    using System;

    public class Knight : Piece
    {
        private int[,] _LegalMoves = new int[8, 2]
        {
            { -1, 2},
            { 1, 2},

            { -1, -2},
            { 1, -2},

            { -2, 1},
            { -2, -1},

            { 2, 1},
            { 2, -1}
        };

        public int[,] GenerateLegalMoves()
        {
            return _LegalMoves;
        }
    }
}