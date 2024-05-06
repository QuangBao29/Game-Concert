using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Scriptable Object/Scene Data")]
public class SceneData : ScriptableObject
{
    public List<AssetReference> ListSceneReference;
}
