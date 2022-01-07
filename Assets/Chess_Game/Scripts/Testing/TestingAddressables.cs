namespace Chess.Testing
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    using System.Threading.Tasks;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public class TestingAddressables : MonoBehaviour
    {
        private const string BISHOP_PREFAB_ADDRESS = "Bishop";

        // Update is called once per frame
        private async void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
                await SpawnPieces();
        }

        private async Task Spawn()
        {
            //AsyncOperationHandle<GameObject> bishop = Addressables.LoadAssetAsync<GameObject>(BISHOP_PREFAB_ADDRESS);
            AsyncOperationHandle<GameObject> bishop = Addressables.InstantiateAsync(BISHOP_PREFAB_ADDRESS);
            GameObject obj = await bishop.Task;
            await Task.Delay(2000);
            Addressables.ReleaseInstance(obj);
        }

        private async Task SpawnBishop()
        {
            GameObject bishop = await LoadBishop<GameObject>();
            GameObject obj = Instantiate(bishop);
            await Task.Delay(2000);
            Destroy(obj);
        }

        private async Task<T> LoadBishop<T>() where T : Object
        {
            AsyncOperationHandle<T> bishop = Addressables.LoadAssetAsync<T>(BISHOP_PREFAB_ADDRESS);
            await bishop.Task;
            T obj = bishop.Result;
            Addressables.Release(bishop);
            return obj;
        }

        private async Task SpawnPieces()
        {
            List<GameObject> Pieces = await LoadAllPieces<GameObject>("Pieces");
            foreach (GameObject piece in Pieces)
                Instantiate(piece);
        }

        //TODO: convert this into a more generic system and give it an option for a callback taht is default null
        private async Task<List<T>> LoadAllPieces<T>(string label) where T : Object
        {
            AsyncOperationHandle<IList<T>> Pieces = Addressables.LoadAssetsAsync<T>(label, null);
            await Pieces.Task;
            List<T> list = new(Pieces.Result);
            return list;
        }

        private async Task<List<T>> LoadAllPieces<T>(List<string> labels) where T : Object
        {
            AsyncOperationHandle<IList<T>> Pieces = Addressables.LoadAssetsAsync<T>(labels, obj => Debug.Log(obj.name));
            await Pieces.Task;
            List<T> list = new(Pieces.Result);
            return list;
        }
    }
}