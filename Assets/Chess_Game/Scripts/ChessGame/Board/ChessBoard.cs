namespace Chess
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using UnityEngine;
    using UnityEngine.AddressableAssets;

    using Pieces;
    using camera;

    public class ChessBoard : MonoBehaviour
    {
        // Fields
        [SerializeField]
        private GameObject _BoardContainer;
        [SerializeField]
        private GameObject _WhitePiecesContainer;
        [SerializeField]
        private GameObject _BlackPiecesContainer;

        private List<Material> _NormalNodeMaterials;

        private EPiece[,] _BoardPieces = new EPiece[8, 8]
        {
            {EPiece.Rook, EPiece.Knight, EPiece.Bishop, EPiece.King, EPiece.Queen, EPiece.Bishop, EPiece.Knight, EPiece.Rook},
            {EPiece.Pawn, EPiece.Pawn, EPiece.Pawn, EPiece.Pawn, EPiece.Pawn, EPiece.Pawn, EPiece.Pawn, EPiece.Pawn},
            {EPiece.Empty, EPiece.Empty, EPiece.Empty, EPiece.Empty, EPiece.Empty, EPiece.Empty, EPiece.Empty, EPiece.Empty},
            {EPiece.Empty, EPiece.Empty, EPiece.Empty, EPiece.Empty, EPiece.Empty, EPiece.Empty, EPiece.Empty, EPiece.Empty},
            {EPiece.Empty, EPiece.Empty, EPiece.Empty, EPiece.Empty, EPiece.Empty, EPiece.Empty, EPiece.Empty, EPiece.Empty},
            {EPiece.Empty, EPiece.Empty, EPiece.Empty, EPiece.Empty, EPiece.Empty, EPiece.Empty, EPiece.Empty, EPiece.Empty},
            {EPiece.Pawn, EPiece.Pawn, EPiece.Pawn, EPiece.Pawn, EPiece.Pawn, EPiece.Pawn, EPiece.Pawn, EPiece.Pawn},
            {EPiece.Rook, EPiece.Knight, EPiece.Bishop, EPiece.Queen, EPiece.King, EPiece.Bishop, EPiece.Knight, EPiece.Rook}
        };

        private BoardNode[,] _GameBoard = new BoardNode[8, 8];

        public static EPieceColor CurrentTurn = EPieceColor.None;

        public static int TurnsElapsed;

        private async void Start()
        {
            CameraMovement.instance.Init();

            _NormalNodeMaterials = new(await AddressablesUtils.LoadAssetsAsyncAndReleaseHandle<Material>(new List<string> { "Board", "Normal" }));

            await SetupGrid();

            CurrentTurn = EPieceColor.White;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
                ChangeTurn();
        }

        private async Task SetupGrid()
        {
            GameObject boardNodePrefab = await AddressablesUtils.LoadAssetAsyncAndReleaseHandle<GameObject>("BoardNode");

            GameObject quad;
            GridNode gridNode;

            GameObject instantiatedPiece;
            Piece piece;
            EPieceColor pieceColor;

            for (int x = 0; x < _GameBoard.GetLength(0); x++)
            {
                GameObject rowObject = _BoardContainer.transform.GetChild(x).gameObject;

                for (int y = 0; y < _GameBoard.GetLength(1); y++)
                {
                    // Board Generation
                    quad = Instantiate(boardNodePrefab, new Vector3(y, 0, x), boardNodePrefab.transform.rotation, rowObject.transform);
                    quad.name = $"{rowObject.name}{y + 1}";
                    quad.layer = LayerMask.NameToLayer("Board");

                    gridNode = quad.AddComponent<GridNode>();
                    gridNode.Init(_NormalNodeMaterials[(x + y) % 2], _BoardPieces[x, y] != EPiece.Empty);

                    // Piece Generation
                    if (_BoardPieces[x, y] == EPiece.Empty)
                    {
                        _GameBoard[x, y] = new BoardNode(null, gridNode);
                        continue;
                    }

                    pieceColor = x < 2 ? EPieceColor.White : EPieceColor.Black;

                    instantiatedPiece = Instantiate(await AddressablesUtils.LoadAssetAsyncAndReleaseHandle<GameObject>(_BoardPieces[x, y].ToString()));
                    piece = instantiatedPiece.AddComponent<Piece>();
                    piece.Init(_BoardPieces[x, y], pieceColor, new Vector3(y, .1f, x), x > 2 ? 180 : 0, CheckIfPlayerTurn, GetPieceCurrentPosition, ShowLegalMoves);
                    piece.transform.SetParent(pieceColor == EPieceColor.White ? _WhitePiecesContainer.transform : _BlackPiecesContainer.transform);

                    _GameBoard[x, y] = new BoardNode(piece, gridNode);
                }
            }
        }

        public bool CheckIfPlayerTurn(EPieceColor pieceColor)
        {
            return CurrentTurn.Equals(pieceColor);
        }

        public async void ChangeTurn()
        {
            TurnsElapsed++;
            CurrentTurn = EPieceColor.None;
            int nextTurn = TurnsElapsed % 2;
            await CameraMovement.instance.RotateCamera((EPieceColor)nextTurn);
            CurrentTurn = (EPieceColor)nextTurn;
        }

        public Vector2Int GetPieceCurrentPosition(Piece piece)
        {
            for (int x = 0; x < _GameBoard.GetLength(0); x++)
            {
                for (int y = 0; y < _GameBoard.GetLength(1); y++)
                {
                    if (_GameBoard[x, y].Piece != piece)
                        continue;

                    return new Vector2Int(y, x);
                }
            }

            return new Vector2Int(0, 0);
        }

        public void ShowLegalMoves(Vector2Int[] legalMoves)
        {
            BoardNode.NormalizeNode.Invoke();

            foreach(Vector2Int legalMove in legalMoves)
            {
                _GameBoard[legalMove.y, legalMove.x].HighlightGridNode();
            }
        }
    }
}