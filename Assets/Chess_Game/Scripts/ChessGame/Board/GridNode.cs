namespace Chess
{
    using UnityEngine;

    using UnityEngine.AddressableAssets;

    /// <summary>Represents a square on the chess board </summary>
    public class GridNode : MonoBehaviour
    {
        private MeshRenderer _MeshRenderer;

        private Material _NormalMaterial;
        private Material _NodeHighlightMaterial;

        private bool _IsOccupied;

        public async void Init(Material normalMaterial, bool isOccupied)
        {
            _NormalMaterial = normalMaterial;
            _NodeHighlightMaterial = await AddressablesUtils.LoadAssetAsyncAndReleaseHandle<Material>("MovementNodes");
            _IsOccupied = isOccupied;

            _MeshRenderer = gameObject.GetComponent<MeshRenderer>();

            SetNodeToNormal();
        }

        public void OnMouseDown()
        {
            throw new System.NotImplementedException();
        }

        public void SetNodeToNormal()
        {
            _MeshRenderer.material = _NormalMaterial;
        }

        public void HighlightNode()
        {
            _MeshRenderer.material = _NodeHighlightMaterial;
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