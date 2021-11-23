namespace Chess.Pieces
{
    using System;

    using UnityEngine;

    public class Piece : MonoBehaviour
    {

        private int GridX;

        private int GridY;

        private Material _NormalMaterial;
        private Material _HighlightedMaterial;

        public void Init(int gridX, int gridY, Material normalMat, Material highlightedMat)
        {
            GridX = gridX;
            GridY = gridY;
            _NormalMaterial = normalMat;
            _HighlightedMaterial = highlightedMat;
        }


        // Behaviour Methods
        public void OnMouseEnter()
        {
            GetComponent<MeshRenderer>().material = _HighlightedMaterial;
        }

        public void OnMouseExit()
        {
            GetComponent<MeshRenderer>().material = _NormalMaterial;
        }
    }
}