using PlayFab;
using UnityEngine;

public class PlayFabErrorHandler : PersistentManager<PlayFabErrorHandler>
{
    public GameEvent playFabError;

    public void PlayFabError(string errorMessage)
    {
        playFabError.Invoke(this, null);
    }

    public static void HandleError(PlayFabError error)
    {
        Debug.LogWarning(error.GenerateErrorReport());
    }
}