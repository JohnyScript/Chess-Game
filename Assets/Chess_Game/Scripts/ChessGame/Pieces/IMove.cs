namespace Chess.Pieces
{
    using UnityEngine;

    public interface IMove
    {
        public Vector2Int[] GetLegalMoves(Vector2Int currentPosition);

        public void MovePiece(Vector2Int destination);
    }
}