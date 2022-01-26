namespace Chess
{
    using System;

    using Pieces;

    public class BoardNode
    {
        public Piece Piece { get; private set; }

        public GridNode Node { get; private set; }

        public BoardNode(Piece piece, GridNode node)
        {
            Piece = piece;
            Node = node;
        }

        /// <summary>Sets if the current board square has a piece on top of it</summary>
        /// <param name="piece">The piece that moved or entered this square</param>
        public void SetNodeState(Piece piece = null)
        {
            Piece = piece;
            Node.GetOrSetState(piece != null);
        }
    }
}