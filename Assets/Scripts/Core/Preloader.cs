using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class Preloader : MonoBehaviour
{
    public bool clearCache;
    public AssetReference sceneRef;

    private async void Start()
    {
        if (clearCache)
            Caching.ClearCache();

        // var resourceLocator = await Addressables.InitializeAsync().Task;
        // var allKeys = resourceLocator.Keys.ToList();
        //
        // foreach (var key in allKeys)
        // {
        //     var keyDownloadSizeKb = await Addressables.GetDownloadSizeAsync(key).Task;
        //     if (keyDownloadSizeKb <= 0) continue;
        //
        //     var keyDownloadOperation = Addressables.DownloadDependenciesAsync(key);
        //     while (!keyDownloadOperation.IsDone)
        //     {
        //         await Task.Yield();
        //     }
        // }

        var handleSo = Addressables.LoadAssetAsync<ScriptableObject>("event");
        await handleSo.Task;

        var persistentManagers = Resources.Load<PersistentManagers>("Scriptable Objects/Persistent Managers");

        foreach (var manager in persistentManagers.listManagers)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(manager);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var managerPrefab = handle.Result;
                Instantiate(managerPrefab);
            }

            Addressables.Release(handle);
        }

        var handleScene = Addressables.LoadSceneAsync(sceneRef, LoadSceneMode.Additive);
        await handleScene.Task;
        
        if (handleScene.Status == AsyncOperationStatus.Succeeded)
        {
            handleScene.Result.ActivateAsync();
            ResourceManager.Instance.SceneHandle = handleScene;
        }
    }
}