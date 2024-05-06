using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEditor.Animations;

public class ResourceManager : PersistentManager<ResourceManager>
{
    private static readonly List<AsyncOperationHandle> ListHandle = new();
    private static AsyncOperationHandle _sceneHandle;

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

    public static AnimatorController LoadAnimator(string animatorPath)
    {
        var handle = Addressables.LoadAssetAsync<AnimatorController>(animatorPath);
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
        _sceneHandle = handleScene;
        handleScene.WaitForCompletion();

        if (handleScene.Status == AsyncOperationStatus.Succeeded)
        {
            handleScene.Result.ActivateAsync();
        }
    }

    public void UnloadScene()
    {
        Addressables.UnloadSceneAsync(_sceneHandle);
    }
}