using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class ResourceManager : PersistentManager<ResourceManager>
{
    private static readonly List<AsyncOperationHandle> ListHandle = new();
    public AsyncOperationHandle SceneHandle;

    private void Start()
    {
        Application.quitting += ClearHandle;
    }

    public static GameObject LoadPrefabAsset(AssetReference prefabReference)
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(prefabReference);
        handle.WaitForCompletion();
        ListHandle.Add(handle);
        return handle.Status == AsyncOperationStatus.Succeeded ? handle.Result : null;
    }

    public static GameObject LoadPrefabAsset(string prefabPath)
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(prefabPath);
        handle.WaitForCompletion();
        ListHandle.Add(handle);
        return handle.Status == AsyncOperationStatus.Succeeded ? handle.Result : null;
    }

    public static void UnloadPrefabAsset(GameObject gameObject)
    {
        Addressables.ReleaseInstance(gameObject);
    }


    public static void UnloadAudioClipAsset(AudioClip audioClip)
    {
        Addressables.Release(audioClip);
    }


    public static void UnloadSpriteAsset(Sprite sprite)
    {
        Addressables.Release(sprite);
    }

    public static AudioClip LoadAudioClip(string audioPath)
    {
        var handle = Addressables.LoadAssetAsync<AudioClip>(audioPath);
        handle.WaitForCompletion();
        ListHandle.Add(handle);
        return handle.Status == AsyncOperationStatus.Succeeded ? handle.Result : null;
    }

    public static Sprite LoadSprite(string spritePath)
    {
        var handle = Addressables.LoadAssetAsync<Sprite>(spritePath);
        handle.WaitForCompletion();
        ListHandle.Add(handle);
        return handle.Status == AsyncOperationStatus.Succeeded ? handle.Result : null;
    }

    public static RuntimeAnimatorController LoadAnimator(string animatorPath)
    {
        var handle = Addressables.LoadAssetAsync<RuntimeAnimatorController>(animatorPath);
        handle.WaitForCompletion();
        ListHandle.Add(handle);
        return handle.Status == AsyncOperationStatus.Succeeded ? handle.Result : null;
    }

    public static AnimationClip LoadAnimationClip(string animationClipPath)
    {
        var handle = Addressables.LoadAssetAsync<AnimationClip>(animationClipPath);
        handle.WaitForCompletion();
        ListHandle.Add(handle);
        return handle.Status == AsyncOperationStatus.Succeeded ? handle.Result : null;
    }

    private static void ClearHandle()
    {
        foreach (var handle in ListHandle)
        {
            if (handle.IsValid())
            {
                Addressables.Release(handle);
            }
        }
    }

    public void LoadScene(int sceneIndex)
    {
        var sceneData = Resources.Load<SceneData>("Scriptable Objects/Scene Data");
        var handleScene = Addressables.LoadSceneAsync(sceneData.ListSceneReference[sceneIndex], LoadSceneMode.Additive);
        SceneHandle = handleScene;
        handleScene.WaitForCompletion();

        if (handleScene.Status == AsyncOperationStatus.Succeeded)
        {
            handleScene.Result.ActivateAsync();
        }
    }

    public void UnloadScene()
    {
        Addressables.UnloadSceneAsync(SceneHandle);
    }

    public async Task<AudioClip> LoadLocalAudioClip(string filePath)
    {
        string uri = "file://" + filePath;

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(uri, GetAudioType(filePath)))
        {
            var operation = www.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield();
            }

            if (www.result == UnityWebRequest.Result.Success)
            {
                return DownloadHandlerAudioClip.GetContent(www);
            }
            else
            {
                Debug.LogError("Failed to load Audio Clip: " + www.error);
                return null;
            }
        }
    }


    private AudioType GetAudioType(string filePath)
    {
        if (filePath.EndsWith(".wav", System.StringComparison.OrdinalIgnoreCase))
        {
            return AudioType.WAV;
        }
        else if (filePath.EndsWith(".mp3", System.StringComparison.OrdinalIgnoreCase))
        {
            return AudioType.MPEG;
        }
        else
        {
            Debug.LogError("Unsupported audio format: " + filePath);
            return AudioType.UNKNOWN;
        }
    }
}