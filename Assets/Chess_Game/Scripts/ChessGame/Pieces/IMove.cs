namespace Chess.Pieces
{
    using UnityEngine;

    public interface IMove
    {
        public Vector2Int[][] GetLegalMoves();

        public void MovePiece(Vector2Int destination);
    }
}