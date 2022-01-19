namespace Chess.Pieces
{
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.AddressableAssets;

    using Enums;

    [RequireComponent(typeof(Rigidbody))]
    public class Piece : MonoBehaviour, IMove
    {
        // Vector2Int [Max number of possibles for each EPiece][2 -> {current position}, {destination}]
        // Example:
        //      Bishop, [4][2] {
        //                       {{3, 3}, {0, 0}},
        //                       {{3, 3}, {6, 0}},
        //                       {{3, 3}, {0, 6}},
        //                       {{3, 3}, {7, 7}}
        //                      }
        private static Dictionary<EPieceColor, Dictionary<EPiece, Vector2Int[][]>> _CurrentLegalMoves;

        // Delegates
        public delegate bool TurnCheck(EPieceColor pieceColor);
        public delegate void CacheCear();

        // Fields
        private EPieceColor _PieceColor;
        private EPiece _PieceType;

        private MeshRenderer _MeshRenderer;

        private Material _NormalMaterial;
        private Material _HighlightedMaterial;

        private event TurnCheck OnTurnCheck;
        private static event CacheCear OnCacheClear;

        public async void Init(EPiece pieceType, EPieceColor pieceColor, Vector3 startingPosition, int startingRotation, TurnCheck turnCheck)
        {
            _PieceType = pieceType;
            _PieceColor = pieceColor;

            transform.position = startingPosition;
            transform.rotation = Quaternion.Euler(0, startingRotation, 0);

            OnTurnCheck = turnCheck;
            if (OnCacheClear.GetInvocationList().Length < 1)
                OnCacheClear = ClearCachedLegalMoves;

            _MeshRenderer = GetComponent<MeshRenderer>();

            _NormalMaterial = await AddressablesUtils.LoadAssetAsyncAndReleaseHandle<Material>($"{pieceColor}Pieces");
            _HighlightedMaterial = await AddressablesUtils.LoadAssetAsyncAndReleaseHandle<Material>("HighlightPieces");

            _MeshRenderer.material = _NormalMaterial;
        }

        public void OnMouseEnter()
        {
            if (!OnTurnCheck(_PieceColor) || _MeshRenderer.material == _HighlightedMaterial)
                return;

            _MeshRenderer.material = _HighlightedMaterial;
        }

        public void OnMouseDown()
        {
            if (!OnTurnCheck(_PieceColor))
                return;

            GetLegalMoves();
        }

        public void OnMouseExit()
        {
            if (_MeshRenderer.material == _NormalMaterial)
                return;

            _MeshRenderer.material = _NormalMaterial;
        }

        public void MovePiece(Vector2Int distination)
        {
            throw new System.NotImplementedException();
            OnCacheClear.Invoke();
        }

        public Vector2Int[][] GetLegalMoves()
        {
            throw new System.NotImplementedException();
        }

        private void ClearCachedLegalMoves()
        {
            throw new System.NotImplementedException();
        }
    }
}