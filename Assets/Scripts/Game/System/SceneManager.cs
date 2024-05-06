using EventData;
using UnityEngine;

public class SceneManager : PersistentManager<SceneManager>
{
    public LevelData levelData;

    public void LoadUIScene(Component sender, object data)
    {
        ResourceManager.Instance.UnloadScene();
        ResourceManager.Instance.LoadScene(0);
    }

    public void LoadPlayScene(Component sender, object data)
    {
        levelData = (LevelData)data;
        ResourceManager.Instance.UnloadScene();
        ResourceManager.Instance.LoadScene(1);
    }
}