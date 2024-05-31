using EventData;
using UnityEngine;

public class SceneManager : PersistentManager<SceneManager>
{
    public UserLevelData UserLevelData;
    public LevelData LevelData;

    public void LoadUIScene(Component sender, object data)
    {
        ResourceManager.Instance.UnloadScene();
        ResourceManager.Instance.LoadScene(0);
    }

    public void LoadPlayScene(Component sender, object data)
    {
        LevelData = (LevelData)data;
        UserLevelData.SongPath = "";
        ResourceManager.Instance.UnloadScene();
        ResourceManager.Instance.LoadScene(1);
    }

    public void LoadCustomPlayScene(Component sender, object data)
    {
        UserLevelData = (UserLevelData)data;
        LevelData.SongIndex = -1;
        ResourceManager.Instance.UnloadScene();
        ResourceManager.Instance.LoadScene(1);
    }

    public void ReLoadPlayScene(Component sender, object data)
    {
        ResourceManager.Instance.UnloadScene();
        ResourceManager.Instance.LoadScene(1);
    }
}