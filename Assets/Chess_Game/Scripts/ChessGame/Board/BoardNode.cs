namespace Chess
{
    using System;

    using Pieces;

    public class BoardNode
    {
        public Piece Piece { get; private set; }

        public GridNode Node { get; private set; }

        public static Action NormalizeNode;

        public BoardNode(Piece piece, GridNode node)
        {
            Piece = piece;
            Node = node;
            NormalizeNode += node.SetNodeToNormal;
        }

        /// <summary>Sets if the current board square has a piece on top of it</summary>
        /// <param name="piece">The piece that moved or entered this square</param>
        public void SetNodeState(Piece piece = null)
        {
            Piece = piece;
        }

        public EPieceColor GetPieceColor() => Piece.GetPieceColor();

        public bool GetNodeState() => Piece != null;

        public void HighlightGridNode() => Node.HighlightNode();
    }
}