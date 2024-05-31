using System.Collections.Generic;
using GameData;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptable Object/Custom Level Data")]
public class CustomLevelData : ScriptableObject
{
    public string FolderPath;
    public string SongPath;
    public string MIDIPath;
    public string AnimPath;
    public List<CustomSongData> ListSong;
}
