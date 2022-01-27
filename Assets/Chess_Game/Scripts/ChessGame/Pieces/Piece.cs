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
        public delegate EPieceColor PositionDataCheck(int x, int y);
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
        private PositionDataCheck _OccupiedPositionCheck;
        private event LegalMovesDisplay _OnLegalMovesDisplay;
        private static event CacheCear _OnCacheClear;

        public async void Init(EPiece pieceType, EPieceColor pieceColor, Vector3 startingPosition, int startingRotation)
        {
            _PieceType = pieceType;
            _PieceColor = pieceColor;

            transform.position = startingPosition;
            transform.rotation = Quaternion.Euler(0, startingRotation, 0);

            _MeshRenderer = GetComponent<MeshRenderer>();

            _NormalMaterial = await AddressablesUtils.LoadAssetAsyncAndReleaseHandle<Material>($"{pieceColor}Pieces");
            _HighlightedMaterial = await AddressablesUtils.LoadAssetAsyncAndReleaseHandle<Material>("HighlightPieces");

            _MeshRenderer.material = _NormalMaterial;
        }

        public void AttachListeners(TurnCheck turnCheck, Func<Piece, Vector2Int> piecePositionRequest, PositionDataCheck occupiedPositionCheck, LegalMovesDisplay movesDisplay)
        {
            _OnTurnCheck = turnCheck;
            _OnPiecePositionRequest = piecePositionRequest;
            _OccupiedPositionCheck = occupiedPositionCheck;
            _OnLegalMovesDisplay = movesDisplay;
            _OnCacheClear += ClearCachedLegalMoves;
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

        public EPieceColor GetPieceColor() => _PieceColor;

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

            Vector2Int currentMoveBeingCalculated;

            foreach (Vector2Int pieceMovementPoint in pieceMovement._PieceMovementPoints)
            {
                if (pieceMovement._PieceMovementType == EPieceMovementType.Jump)
                {
                    currentMoveBeingCalculated = currentPosition + pieceMovementPoint;

                    if (IsPositionOutOfBounds(currentMoveBeingCalculated) || IsPositionOccupied(currentMoveBeingCalculated) == _PieceColor)
                        continue;

                    legalMoves.Add(currentMoveBeingCalculated);

                    continue;
                }

                for (int currentMove = 1; currentMove <= 8; currentMove++)
                {
                    currentMoveBeingCalculated = currentPosition + (pieceMovementPoint * currentMove);

                    if (IsPositionOutOfBounds(currentMoveBeingCalculated) || IsPositionOccupied(currentMoveBeingCalculated) == _PieceColor)
                        break;

                    legalMoves.Add(currentMoveBeingCalculated);

                    // Hate this line but as the Enum stands it's the current only way
                    if (IsPositionOccupied(currentMoveBeingCalculated) != EPieceColor.None)
                        break;
                }
            }

            _CachedLegalMoves = legalMoves.ToArray();
            _OnLegalMovesDisplay.Invoke(_CachedLegalMoves);
            return _CachedLegalMoves;
        }

        private bool IsPositionOutOfBounds(Vector2Int positionBeingChecked)
        {
            if (positionBeingChecked.x >= 8 || positionBeingChecked.x < 0)
                return true;

            if (positionBeingChecked.y >= 8 || positionBeingChecked.y < 0)
                return true;

            return false;
        }

        private EPieceColor IsPositionOccupied(Vector2Int positionToCheck)
        {
            return _OccupiedPositionCheck.Invoke(positionToCheck.x, positionToCheck.y);
        }

        private void ClearCachedLegalMoves()
        {
            _CachedLegalMoves = null;
        }
    }
}