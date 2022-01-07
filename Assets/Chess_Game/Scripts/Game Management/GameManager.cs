namespace Chess.Managers
{
    using UnityEngine;
    using System.Threading.Tasks;

    using Grid;
    using Pieces;
    using Enums;
    using camera;

    public class GameManager : MonoBehaviour
    {
        // Constants
        private const string BOARD_MATERIALS_PATH = "Materials/Board/Normal";
        private const string PIECES_MOVEMENT_MATERIAL_PATH = "Materials/Board/Movement";
        private const string PIECES_PREFABS_PATH = "Prefabs/Pieces/";

        // Fields
        private Material[] _NormalNodeMaterials;
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

            _NormalNodeMaterials = Resources.LoadAll<Material>(BOARD_MATERIALS_PATH);
            _NodeHighlightMaterial = Resources.LoadAll<Material>(PIECES_MOVEMENT_MATERIAL_PATH)[0];

            await SetupGrid();
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
                ChangeTurn();
        }

        private async Task SetupGrid()
        {
            GameObject boardObject = new GameObject("Board");
            boardObject.transform.SetParent(transform);

            GameObject pieceContainer = new GameObject("Piece Container");
            pieceContainer.transform.SetParent(transform);
            GameObject whites = new GameObject("Whites");
            GameObject blacks = new GameObject("Blacks");

            whites.transform.SetParent(pieceContainer.transform);
            blacks.transform.SetParent(pieceContainer.transform);

            GridNode gridNode;

            GameObject quad;
            char boardLetter;

            GameObject instantiatedPiece;
            Piece piece;
            EPieceColor pieceColor;

            for (int x = 0; x < _GameBoard.GetLength(0); x++)
            {
                boardLetter = (char)(65 + x);

                GameObject rowObject = new GameObject($"{boardLetter}");
                rowObject.transform.SetParent(boardObject.transform);

                for (int y = 0; y < _GameBoard.GetLength(1); y++)
                {
                    // Board Generation
                    quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    quad.transform.rotation = Quaternion.Euler(90, 0, 0);
                    quad.transform.position = new Vector3(y, 0, x);
                    quad.name = $"{boardLetter}{y + 1}";
                    quad.transform.SetParent(rowObject.transform);
                    quad.layer = LayerMask.NameToLayer("Board");

                    gridNode = quad.AddComponent<GridNode>();
                    //y < 2 || y > _Grid.GetLength(0) - 2 are the rows occupied by the white and black pieces respectfully
                    gridNode.Init(_NormalNodeMaterials[(x + y) % 2], y < 2 || y > _GameBoard.GetLength(0) - 2);

                    // Piece Generation
                    if (_BoardPieces[x, y] == EPiece.Empty)
                        continue;

                    pieceColor = x < 2 ? EPieceColor.White : EPieceColor.Black;

                    instantiatedPiece = Instantiate(Resources.Load<GameObject>(PIECES_PREFABS_PATH + _BoardPieces[x, y].ToString()));
                    piece = instantiatedPiece.AddComponent<Piece>();
                    piece.Init(pieceColor, _BoardPieces[x, y], new Vector3(y, .1f , x), x > 2 ? 180 : 0, CheckIfPlayerTurn);
                    piece.transform.SetParent(pieceColor == EPieceColor.White ? whites.transform : blacks.transform);

                    _GameBoard[x, y] = new BoardNode(piece, gridNode);
                }
            }
        }

        public bool CheckIfPlayerTurn(EPieceColor pieceColor)
        {
            return CurrentTurn.Equals(pieceColor);
        }

        public void ChangeTurn()
        {
            TurnsElapsed++;
            CurrentTurn = (EPieceColor)(TurnsElapsed % 2);
            CameraMovement.instance.RotateCamera(CurrentTurn);
        }

        public void ShowLegalMoves(int[,] legalMoves)
        {
            //for(int i = 0)
        }
    }
}