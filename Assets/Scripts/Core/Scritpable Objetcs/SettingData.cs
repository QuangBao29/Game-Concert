using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/SettingData")]
public class SettingData : ScriptableObject
{
    public List<string> ListSetting;
    public List<string> ListData;
}