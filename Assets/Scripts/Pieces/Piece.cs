namespace Chess.Pieces
{
    using System;

    using UnityEngine;

    using Managers;
    using Enums;

    [RequireComponent(typeof(Rigidbody))]
    public class Piece : MonoBehaviour
    {
        // Delegates
        public delegate bool TurnCheck(EPieceColor pieceColor);

        // Constants
        private const string PIECE_MATERIALS_PATH = "Materials/Pieces/";

        [HideInInspector]
        public EPieceColor PieceColor { get; private set; }

        private MeshRenderer _MeshRenderer;

        private Material _NormalMaterial;
        private Material _HighlightedMaterial;

        private event TurnCheck OnTurnCheck;

        public void Init(EPieceColor pieceColor, Vector3 startingPosition, int startingRotation, TurnCheck turnCheck)
        {
            PieceColor = pieceColor;

            transform.position = startingPosition;
            transform.rotation = Quaternion.Euler(0, startingRotation, 0);

            OnTurnCheck = turnCheck;

            _MeshRenderer = GetComponent<MeshRenderer>();

            _NormalMaterial = Resources.Load<Material>(PIECE_MATERIALS_PATH + pieceColor.ToString());
            _HighlightedMaterial = Resources.Load<Material>(PIECE_MATERIALS_PATH + "Highlighted");

            _MeshRenderer.material = _NormalMaterial;
        }

        // Behaviour Methods
        public void OnMouseEnter()
        {
            if (!OnTurnCheck(PieceColor) || _MeshRenderer.material == _HighlightedMaterial)
                return;

            _MeshRenderer.material = _HighlightedMaterial;
        }

        public void OnMouseExit()
        {
            if (_MeshRenderer.material == _NormalMaterial)
                return;

            _MeshRenderer.material = _NormalMaterial;
        }

    }
}