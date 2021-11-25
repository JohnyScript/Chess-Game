namespace Chess.Managers
{
    using Pieces;
    using Grid;

    public class BoardNode
    {
        public Piece Piece { get; private set; }

        public GridNode Node { get; private set; }

        public BoardNode(Piece piece, GridNode node)
        {
            Piece = piece;
            Node = node;
        }


    }
}