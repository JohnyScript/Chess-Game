namespace Chess.Pieces
{
    using System;

    using UnityEngine;

    public class PieceMovement : IEquatable<PieceMovement>
    {
        public EPieceMovementType _PieceMovementType { get; private set; }

        /// <summary>
        /// This Array contians all of the Directions or Jump points of each piece
        /// </summary>
        public Vector2Int[] _PieceMovementPoints { get; private set; }

        public PieceMovement(EPieceMovementType pieceMovementType, Vector2Int[] pieceMovementPoints)
        {
            _PieceMovementType = pieceMovementType;
            _PieceMovementPoints = pieceMovementPoints;
        }

        public bool Equals(PieceMovement other)
        {
            if (_PieceMovementType != other._PieceMovementType) return false;
            if (_PieceMovementPoints.Length != other._PieceMovementPoints.Length) return false;

            for(int i = 0; i < _PieceMovementPoints.Length; i++)
            {
                if (_PieceMovementPoints[i] != other._PieceMovementPoints[i]) return false;
            }

            return true;
        }
    }
}