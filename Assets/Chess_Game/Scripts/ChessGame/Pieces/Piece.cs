namespace Chess.Pieces
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [RequireComponent(typeof(Rigidbody))]
    public class Piece : MonoBehaviour, IMove
    {
        private static Dictionary<EPiece, PieceMovement> _PieceMovementPointsByPieceType = new(8)
        {
            {
                EPiece.Bishop,
                new PieceMovement(EPieceMovementType.Slide,
                                  new Vector2Int[4]
                                  {
                                      new (1, 1),
                                      new (1, -1),
                                      new (-1, -1),
                                      new (-1, 1)
                                  })
            },

            {
                EPiece.King,
                new PieceMovement(EPieceMovementType.Jump,
                                  new Vector2Int[8]
                                  {
                                      new (0, 1),
                                      new (1, 1),
                                      new (1, 0),
                                      new (1, -1),
                                      new (0, -1),
                                      new (-1, -1),
                                      new (-1, 0),
                                      new (-1, 1)
                                  })
            },

            {
                EPiece.Knight,
                new PieceMovement(EPieceMovementType.Jump,
                                  new Vector2Int[8]
                                  {
                                      new (1, 2),
                                      new (2, 1),
                                      new (2, -1),
                                      new (1, -2),
                                      new (-1, -2),
                                      new (-2, -1),
                                      new (-2, 1),
                                      new (-1, 2)
                                  })
            },

            {
                EPiece.Pawn,
                new PieceMovement(EPieceMovementType.Jump,
                                  new Vector2Int[4]
                                  {
                                      new (0, 1),
                                      new (0, 2),
                                      new (-1, 1),
                                      new (1, 1)
                                  })
            },

            {
                EPiece.Queen,
                new PieceMovement(EPieceMovementType.Slide,
                                  new Vector2Int[8]
                                  {
                                      new (0, 1),
                                      new (1, 1),
                                      new (1, 0),
                                      new (1, -1),
                                      new (0, -1),
                                      new (-1, -1),
                                      new (-1, 0),
                                      new (-1, 1)
                                  })
            },

            {
                EPiece.Rook,
                new PieceMovement(EPieceMovementType.Slide,
                                  new Vector2Int[4]
                                  {
                                      new (0, 1),
                                      new (1, 0),
                                      new (0, -1),
                                      new (-1, 0)
                                  })
            }
        };

        // Delegates
        public delegate bool TurnCheck(EPieceColor pieceColor);
        public delegate void LegalMovesDisplay(Vector2Int[] allLegalMoves);
        public delegate void CacheCear();

        // Fields
        private EPieceColor _PieceColor;
        private EPiece _PieceType;

        private MeshRenderer _MeshRenderer;

        private Material _NormalMaterial;
        private Material _HighlightedMaterial;

        private Vector2Int[] _CachedLegalMoves;

        private event TurnCheck _OnTurnCheck;
        private Func<Piece, Vector2Int> _OnPiecePositionRequest;
        private event LegalMovesDisplay _OnLegalMovesDisplay;
        private static event CacheCear _OnCacheClear;

        public async void Init(EPiece pieceType, EPieceColor pieceColor, Vector3 startingPosition, int startingRotation, TurnCheck turnCheck, Func<Piece, Vector2Int> piecePositionRequest, LegalMovesDisplay movesDisplay)
        {
            _PieceType = pieceType;
            _PieceColor = pieceColor;

            transform.position = startingPosition;
            transform.rotation = Quaternion.Euler(0, startingRotation, 0);

            _OnTurnCheck = turnCheck;
            _OnPiecePositionRequest = piecePositionRequest;
            _OnLegalMovesDisplay = movesDisplay;
            _OnCacheClear += ClearCachedLegalMoves;

            _MeshRenderer = GetComponent<MeshRenderer>();

            _NormalMaterial = await AddressablesUtils.LoadAssetAsyncAndReleaseHandle<Material>($"{pieceColor}Pieces");
            _HighlightedMaterial = await AddressablesUtils.LoadAssetAsyncAndReleaseHandle<Material>("HighlightPieces");

            _MeshRenderer.material = _NormalMaterial;
        }

        public void OnMouseEnter()
        {
            if (!_OnTurnCheck(_PieceColor) || _MeshRenderer.material == _HighlightedMaterial)
                return;

            _MeshRenderer.material = _HighlightedMaterial;
        }

        public void OnMouseDown()
        {
            if (!_OnTurnCheck(_PieceColor))
                return;

            GetLegalMoves(_OnPiecePositionRequest(this));
        }

        public void OnMouseExit()
        {
            if (_MeshRenderer.material == _NormalMaterial)
                return;

            _MeshRenderer.material = _NormalMaterial;
        }

        public void MovePiece(Vector2Int distination)
        {
            throw new NotImplementedException();

            _OnCacheClear.Invoke();
        }

        public Vector2Int[] GetLegalMoves(Vector2Int currentPosition)
        {
            // If the player has already selected this Piece then only the cached LegalMoves need to be return
            if (_CachedLegalMoves != null)
            {
                _OnLegalMovesDisplay.Invoke(_CachedLegalMoves);
                return _CachedLegalMoves;
            }

            PieceMovement pieceMovement = _PieceMovementPointsByPieceType[_PieceType];
            List<Vector2Int> legalMoves = new();

            if(pieceMovement._PieceMovementType == EPieceMovementType.Jump)
            {
                for (int i = 0; i < pieceMovement._PieceMovementPoints.Length; i++)
                {
                   legalMoves.Add(currentPosition + pieceMovement._PieceMovementPoints[i]);
                }

                _CachedLegalMoves = legalMoves.ToArray();
                _OnLegalMovesDisplay.Invoke(_CachedLegalMoves);
                return _CachedLegalMoves;
            }

            Vector2Int currentMoveBeingCalculated;
            Vector2Int[] currentPieceMovementPoints = pieceMovement._PieceMovementPoints;

            for (int index = 0; index < currentPieceMovementPoints.Length; index++)
            {
                for (int currentMove = 1; currentMove <= 8; currentMove++)
                {
                    currentMoveBeingCalculated = currentPosition + (currentPieceMovementPoints[index] * currentMove);

                    if (IsPositionOutOfBounds(currentMoveBeingCalculated))
                        break;

                    legalMoves.Add(currentMoveBeingCalculated);

                }
            }

            _CachedLegalMoves = legalMoves.ToArray();
            _OnLegalMovesDisplay.Invoke(_CachedLegalMoves);
            return _CachedLegalMoves;
        }

        private bool IsPositionOutOfBounds(Vector2Int currentPosition)
        {
            if (currentPosition.x >= 8 || currentPosition.x < 0)
                return true;

            if (currentPosition.y >= 8 || currentPosition.y < 0)
                return true;

            return false;
        }

        private void ClearCachedLegalMoves()
        {
            _CachedLegalMoves = null;
        }
    }
}