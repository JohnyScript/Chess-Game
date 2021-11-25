namespace Chess.Pieces
{
    using System;

    using UnityEngine;

    using Enums;

    [RequireComponent(typeof(Rigidbody))]
    public class Piece : MonoBehaviour
    {
        private const string PIECE_MATERIALS_PATH = "Materials/Pieces/";

        [HideInInspector]
        public EPieceColor _PieceColor { get; private set; }

        private MeshRenderer _MeshRenderer;

        private Material _NormalMaterial;
        private Material _HighlightedMaterial;

        public void Init(EPieceColor pieceColor, Vector3 startingPosition, int startingRotation)
        {
            _PieceColor = pieceColor;

            transform.position = startingPosition;
            transform.rotation = Quaternion.Euler(0, startingRotation, 0);

            _MeshRenderer = GetComponent<MeshRenderer>();

            _NormalMaterial = Resources.Load<Material>(PIECE_MATERIALS_PATH + pieceColor.ToString());
            _HighlightedMaterial = Resources.Load<Material>(PIECE_MATERIALS_PATH + "Highlighted");

            _MeshRenderer.material = _NormalMaterial;
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