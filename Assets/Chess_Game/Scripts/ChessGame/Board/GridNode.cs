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

        private bool _IsHighlighted;

        public async void Init(Material normalMaterial, bool isOccupied)
        {
            _NormalMaterial = normalMaterial;
            _NodeHighlightMaterial = await AddressablesUtils.LoadAssetAsyncAndReleaseHandle<Material>("MovementNodes");

            _MeshRenderer = gameObject.GetComponent<MeshRenderer>();

            _MeshRenderer.material = _NormalMaterial;
        }

        public void OnMouseDown()
        {
            if (!_IsHighlighted)
                return;

            throw new System.NotImplementedException();
        }

        public void SetNodeToNormal()
        {
            if (!_IsHighlighted)
                return;

            _MeshRenderer.material = _NormalMaterial;
            _IsHighlighted = false;
        }

        public void HighlightNode()
        {
            if (_IsHighlighted)
                return;

            _MeshRenderer.material = _NodeHighlightMaterial;
            _IsHighlighted = true;
        }
    }
}