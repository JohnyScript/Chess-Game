namespace Chess.Managers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    using Structs;
    using Pieces;
    using Enums;

    public class GameManager : MonoBehaviour
    {
        // Resources to Load
        private const string BOARD_MATERIALS_PATH = "Materials/Board/Normal";
        private const string PIECES_MOVEMENT_MATERIAL_PATH = "Materials/Board/Movement";
        private const string PIECES_PREFABS_PATH = "Prefabs/Pieces/";

        private Material[] _NormalNodeMaterials;
        private Material _MovementNodeMaterial;

        //TODO: Use this to Instantiate the pieces and use indexes to check color
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

        private GridNode[,] _Grid = new GridNode[8, 8];

        private Piece[] _Pieces = new Piece[32];

        private bool _MouseHovering = false;

        private Ray _BoardRay;
        private RaycastHit _RaycastHit;

        private LayerMask mask;

        private void Start()
        {
            _NormalNodeMaterials = Resources.LoadAll<Material>(BOARD_MATERIALS_PATH);
            _MovementNodeMaterial = Resources.LoadAll<Material>(PIECES_MOVEMENT_MATERIAL_PATH)[0];

            mask = ~LayerMask.GetMask("Board");

            SetupGrid();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _Grid[0, 0].Test();
                Debug.Log("Confirmacao: " + _Grid[0, 0]._IsOccupied);
            }
        }
        //Setup Methods

        private void SetupGrid()
        {
            GameObject gridObject = new GameObject("Grid");
            gridObject.transform.SetParent(transform);

            GameObject quad;
            char boardLetter;

            GameObject instantiatedPiece;
            Piece piece;

            for (int x = 0; x < _Grid.GetLength(0); x++)
            {
                boardLetter = (char)(65 + x);

                GameObject rowObject = new GameObject($"{boardLetter}");
                rowObject.transform.SetParent(gridObject.transform);

                for (int y = 0; y < _Grid.GetLength(1); y++)
                {
                    quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    quad.transform.rotation = Quaternion.Euler(90, 0, 0);
                    quad.transform.position = new Vector3(y, 0, x);
                    quad.name = $"{boardLetter}{y + 1}";
                    quad.transform.SetParent(rowObject.transform);
                    quad.layer = LayerMask.NameToLayer("Board");

                    //y < 2 || y > _Grid.GetLength(0) - 2 are the rows occupied by the white and black pieces respectfully
                    _Grid[x, y] = new GridNode(quad.transform, _NormalNodeMaterials[(x + y) % 2], y < 2 || y > _Grid.GetLength(0) - 2);

                    if (y > 2 && y < _Grid.GetLength(0) - 2)
                        continue;

                    //instantiatedPiece = Instantiate(Resources.Load<GameObject>(PIECES_PREFABS_PATH + (x < 2 ? EPieceColor.White.ToString() : EPieceColor.Black.ToString()) + " " +_BoardPieces[x, y].ToString()));
                    //_Pieces[]
                }
            }
        }


        // Behaviour Methods

        // Board Node selection
        /*private void FixedUpdate()
        {
            if (!_MouseHovering)
                return;

            _BoardRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.Log("Raycasting");
            if (Physics.Raycast(_BoardRay, out _RaycastHit, Mathf.Infinity))
            {
                Debug.Log("hit");
                Debug.Log(_RaycastHit.collider.gameObject.name);
            }
        }

        private void OnMouseEnter()
        {
            Debug.Log("IsHovering");
            _MouseHovering = true;
        }

        private void OnMouseExit()
        {
            _MouseHovering = false;
        }*/

        // Grid Coloring Methods

        public void ColorNodes(int gridX, int gridY)
        {
            //TODO: Create PieceBehaviour struct(?) that contains the movement type for each piece
        }
    }
}