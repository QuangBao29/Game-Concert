using PlayFab;

public class PlayFabErrorHandler : PersistentManager<PlayFabErrorHandler>
{
    public GameEvent playFabError;

    private void PlayFabError(string errorMessage)
    {
        playFabError.Invoke(this, errorMessage);
    }


    public void HandleError(PlayFabError error)
    {
        PlayFabError(error.GenerateErrorReport());
    }
}