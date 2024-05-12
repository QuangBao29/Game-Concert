using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GCEditorHelper : Editor
{
    [MenuItem("GC/PlayGame _F5", false, 100)]
    static public void OpenLoginAndStartGC()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/Preloader.unity");
        EditorApplication.ExecuteMenuItem("Edit/Play");
    }

    [MenuItem("GC/SceneEditor/Preloader", false, 4)]
    static public void OpenScenePreloader()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/Preloader.unity");
    }

    [MenuItem("GC/SceneEditor/ScenePlay", false, 4)]
    static public void OpenScenePlay()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/ScenePlay.unity");
    }

    [MenuItem("GC/SceneEditor/MainUI", false, 4)]
    static public void OpenSceneMainUI()
    {
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/Main UI.unity");
    }
}