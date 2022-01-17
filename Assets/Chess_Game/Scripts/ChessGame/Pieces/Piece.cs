namespace Chess.Pieces
{
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    using Enums;

    [RequireComponent(typeof(Rigidbody))]
    public class Piece : MonoBehaviour
    {
        // Delegates
        public delegate bool TurnCheck(EPieceColor pieceColor);

        // Fields
        private EPieceColor _PieceColor;
        private EPiece _PieceType;

        private MeshRenderer _MeshRenderer;

        private Material _NormalMaterial;
        private Material _HighlightedMaterial;

        private event TurnCheck OnTurnCheck;

        public async void Init(EPieceColor pieceColor, EPiece pieceType, Vector3 startingPosition, int startingRotation, TurnCheck turnCheck)
        {
            _PieceColor = pieceColor;
            _PieceType = pieceType;

            transform.position = startingPosition;
            transform.rotation = Quaternion.Euler(0, startingRotation, 0);

            OnTurnCheck = turnCheck;

            _MeshRenderer = GetComponent<MeshRenderer>();

            _NormalMaterial = await AddressablesUtils.LoadAssetAsyncAndReleaseHandle<Material>($"{pieceColor}Pieces");
            _HighlightedMaterial = await AddressablesUtils.LoadAssetAsyncAndReleaseHandle<Material>("HighlightPieces");

            _MeshRenderer.material = _NormalMaterial;
        }

        // Behaviour Methods
        public void OnMouseEnter()
        {
            if (!OnTurnCheck(_PieceColor) || _MeshRenderer.material == _HighlightedMaterial)
                return;

            _MeshRenderer.material = _HighlightedMaterial;
        }

        public void OnMouseExit()
        {
            if (_MeshRenderer.material == _NormalMaterial)
                return;

            _MeshRenderer.material = _NormalMaterial;
        }

        public virtual void MovePiece(Vector3 targetPosition)
        {
            // Implement movment code using bezier curvas?
        }

        //public abstract int[,] GenerateLegalMoves();
    }
}