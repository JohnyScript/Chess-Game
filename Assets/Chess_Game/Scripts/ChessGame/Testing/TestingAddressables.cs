namespace Chess.Testing
{

    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceLocations;

    public class TestingAddressables : MonoBehaviour
    {
        private const string BISHOP_PREFAB_ADDRESS = "Bishop";

        private IList<Material> pieces;

        private async void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
                 await AddressablesUtils.LoadAssetsAsyncAndReleaseHandle<Material>(new List<string>{ "Board", "Normal"}, Addressables.MergeMode.Intersection ,obj => Debug.Log(obj.name));
        }
        
    }
}