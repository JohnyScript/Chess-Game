namespace UnityEngine.AddressableAssets
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ResourceManagement.AsyncOperations;
    using ResourceManagement.ResourceLocations;
    using static AddressableAssets.Addressables;

    public static class AddressablesUtils
    {
        public static async Task<T> LoadAssetAsyncAndReleaseHandle<T>(string assetAddress)
        {
            AsyncOperationHandle<T> loadedAsset = Addressables.LoadAssetAsync<T>(assetAddress);
            await loadedAsset.Task;
            T obj = loadedAsset.Result;
            Addressables.Release(loadedAsset);
            return obj;
        }

        public static async Task<T> LoadAssetAsyncAndReleaseHandle<T>(IResourceLocation resourceLocation)
        {
            AsyncOperationHandle<T> loadedAsset = LoadAssetAsync<T>(resourceLocation);
            await loadedAsset.Task;
            T Obj = loadedAsset.Result;
            Addressables.Release(loadedAsset);
            return Obj;
        }

        public static async Task<IList<T>> LoadAssetsAsyncAndReleaseHandle<T>(string label, MergeMode mergeMode =  MergeMode.Intersection, Action<T> callback = null)
        {
            AsyncOperationHandle<IList<T>> loadedAssets = LoadAssetsAsync(label, callback, mergeMode);
            await loadedAssets.Task;
            IList<T> loadedAssetsList = loadedAssets.Result;
            Addressables.Release(loadedAssets);
            return loadedAssetsList;
        }

        public static async Task<IList<T>> LoadAssetsAsyncAndReleaseHandle<T>(List<string> labels, MergeMode mergeMode = MergeMode.Intersection, Action<T> callback = null)
        {
            AsyncOperationHandle<IList<T>> loadedAssets = LoadAssetsAsync(labels, callback, mergeMode);
            await loadedAssets.Task;
            IList<T> loadedAssetsList = loadedAssets.Result;
            Addressables.Release(loadedAssets);
            return loadedAssetsList;
        }
    }
}