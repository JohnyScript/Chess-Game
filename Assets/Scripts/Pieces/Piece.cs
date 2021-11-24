namespace Chess.Pieces
{
    using System;

    using UnityEngine;

    using Enums;

    public class Piece : MonoBehaviour
    { 
        private const string PIECE_MATERIALS_PATH = "Materials/Pieces/";

        private int GridX;
        private int GridY;

        private MeshRenderer _MeshRenderer;

        private Material _NormalMaterial;
        private Material _HighlightedMaterial;

        public void Init(int gridX, int gridY, EPieceColor pieceColor)
        {
            GridX = gridX;
            GridY = gridY;

            _MeshRenderer = GetComponent<MeshRenderer>();

            _NormalMaterial = Resources.Load<Material>(PIECE_MATERIALS_PATH + pieceColor.ToString());
            _HighlightedMaterial = Resources.Load<Material>(PIECE_MATERIALS_PATH + "Highlighted");
        }

        // Behaviour Methods
        public void OnMouseEnter()
        {
            _MeshRenderer.material = _HighlightedMaterial;
        }

        public void OnMouseExit()
        {
            _MeshRenderer.material = _NormalMaterial;
        }
    }
}