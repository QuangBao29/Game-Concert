using PlayFab;
using UnityEngine;

public class PlayFabErrorHandler : PersistentManager<PlayFabErrorHandler>
{
    public GameEvent playFabError;
    public GameEvent playFabInfo;

    public void PlayFabError(string errorMessage)
    {
        playFabError.Invoke(this, null);
    }

    public void PlayFabInfo(string infoMessage)
    {
        playFabInfo.Invoke(this, null);
    }


    public static void HandleError(PlayFabError error)
    {
        Debug.LogWarning(error.GenerateErrorReport());
    }

    public static void HandleSuccess()
    {
    }
}