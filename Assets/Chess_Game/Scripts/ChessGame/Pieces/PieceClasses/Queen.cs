namespace Chess.Pieces
{
    using Managers;

    using System;

    public class Queen : Piece
    {
        public int[,] _LegalMoves = new int[4, 2]
        {
            { 0, 1},
            { 0, 2},
            { 1, 1},
            { 1, 1}
        };

        public int[,] GenerateLegalMoves()
        {
            return _LegalMoves;
        }
    }
}
