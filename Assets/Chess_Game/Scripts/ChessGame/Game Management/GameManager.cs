namespace Chess.Managers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using UnityEngine;
    using UnityEngine.AddressableAssets;

    using Grid;
    using Pieces;
    using Enums;
    using camera;

    public class GameManager : MonoBehaviour
    {
        // Fields
        [SerializeField]
        private GameObject _BoardContainer;
        [SerializeField]
        private GameObject _WhitePiecesContainer;
        [SerializeField]
        private GameObject _BlackPiecesContainer;

        private List<Material> _NormalNodeMaterials;
        private Material _NodeHighlightMaterial;

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

        // Properties

        public static EPieceColor CurrentTurn = EPieceColor.White;

        public static int TurnsElapsed;

        private async void Start()
        {
            CameraMovement.instance.Init();

            _NormalNodeMaterials = new (await AddressablesUtils.LoadAssetsAsyncAndReleaseHandle<Material>(new List<string>{"Board", "Normal"}));
            _NodeHighlightMaterial = await AddressablesUtils.LoadAssetAsyncAndReleaseHandle<Material>("MovementNodes");

            await SetupGrid();
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
                    gridNode.Init(_NormalNodeMaterials[(x + y) % 2], _BoardPieces[x,y] != EPiece.Empty);

                    // Piece Generation
                    if (_BoardPieces[x, y] == EPiece.Empty)
                        continue;

                    pieceColor = x < 2 ? EPieceColor.White : EPieceColor.Black;

                    instantiatedPiece = Instantiate(await AddressablesUtils.LoadAssetAsyncAndReleaseHandle<GameObject>(_BoardPieces[x, y].ToString()));
                    piece = instantiatedPiece.AddComponent<Piece>();
                    piece.Init(_BoardPieces[x, y], pieceColor, new Vector3(y, .1f , x), x > 2 ? 180 : 0, CheckIfPlayerTurn);
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

        public void ShowLegalMoves(int[,] legalMoves)
        {
            //for(int i = 0)
        }
    }
}