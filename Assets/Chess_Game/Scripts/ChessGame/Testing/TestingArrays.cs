namespace Chess.Testing
{
    using System.Collections;

    using UnityEngine;
    using UnityEngine.AddressableAssets;

    using Chess.Grid;

    public class TestingArrays : MonoBehaviour
    {
        private GridNode _TestingNode;
        private bool _TestingBool = false;

        private async void Start()
        {
            GameObject quad = Instantiate(await AddressablesUtils.LoadAssetAsyncAndReleaseHandle<GameObject>("BoardNode"));
            Material normalMaterial = await AddressablesUtils.LoadAssetAsyncAndReleaseHandle<Material>("WhiteNodes");
            _TestingNode = quad.AddComponent<GridNode>();
            _TestingNode.Init(normalMaterial, _TestingBool);

            GameObject quad2 = Instantiate(await AddressablesUtils.LoadAssetAsyncAndReleaseHandle<GameObject>("BoardNode"));
            GridNode newGridNode = quad2.AddComponent<GridNode>();
            newGridNode.Init(normalMaterial, _TestingBool);

            Debug.Log(_TestingNode.Equals(newGridNode));
            Debug.Log($"Is _TestingNode equal to NewGridNode: {_TestingNode.Equals(newGridNode)}");
        }
    }
}