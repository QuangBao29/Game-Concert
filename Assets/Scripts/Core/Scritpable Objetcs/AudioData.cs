using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Audio Data")]
public class AudioData : ScriptableObject
{
    public string AudioPath;
    public List<string> ListAudio;
}