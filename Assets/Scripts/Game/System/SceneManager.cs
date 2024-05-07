using EventData;
using UnityEngine;

public class SceneManager : PersistentManager<SceneManager>
{
    public LevelData LevelData;

    public void LoadUIScene(Component sender, object data)
    {
        ResourceManager.Instance.UnloadScene();
        ResourceManager.Instance.LoadScene(0);
    }

    public void LoadPlayScene(Component sender, object data)
    {
        LevelData = (LevelData)data;
        ResourceManager.Instance.UnloadScene();
        ResourceManager.Instance.LoadScene(1);
    }

    public void ReLoadPlayScene(Component sender, object data)
    {
        ResourceManager.Instance.UnloadScene();
        ResourceManager.Instance.LoadScene(1);
    }
}