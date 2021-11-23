namespace Chess.Structs
{
    using System;
    using UnityEngine;

    /// <summary>Represents a square on the chess board </summary>
    public struct GridNode
    {
        private Transform _NodeObject;

        private Material _NormalMaterial;

        private bool _IsOccupied;

        public GridNode(Transform nodeObject, Material normalMaterial, bool isOccupied)
        {
            _NodeObject = nodeObject;
            _NormalMaterial = normalMaterial;
            _IsOccupied = isOccupied;

            ChangeNodeMaterial(_NormalMaterial);
        }

        public void ChangeNodeMaterial()
        {
            _NodeObject.GetComponent<MeshRenderer>().material = _NormalMaterial;
        }

        public void ChangeNodeMaterial(Material materialToUse)
        {
            _NodeObject.GetComponent<MeshRenderer>().material = materialToUse;
        }

        /// <summary>Checks or Sets the node's occupied state;</summary>
        public bool GetOrSetState()
        {
            return _IsOccupied;
        }

        /// <summary>Checks or Sets the node's occupied state; </summary>
        /// <param name="isOccupied">The new occupied state of the node</param>
        public void GetOrSetState(bool isOccupied)
        {
            _IsOccupied = isOccupied;
        }
    }
}