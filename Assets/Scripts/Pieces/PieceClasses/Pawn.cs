namespace Chess.Pieces
{
    using Managers;

    using System;

    using UnityEngine;

    public class Pawn : Piece
    {
        public bool FirstMove = false;

        public int[,] _LegalMoves = new int[4, 2]
        {
            { 0, 1},
            { 0, 2},
            { 1, 1},
            { 1, 1}
        };

        public override void MovePiece(Vector3 targetPosition)
        {
            base.MovePiece(targetPosition);

            if (!FirstMove)
                FirstMove = false;
        }

        public int[,] GenerateLegalMoves()
        {
            return _LegalMoves;
        }
    }
}