using System;
using EventData;
using UnityEngine;

public class SettingManager : PersistentManager<SettingManager>
{
    private SettingData _settingData;

    public GameEvent onVolumeUpdated;

    public override void Awake()
    {
        base.Awake();
        _settingData = Resources.Load<SettingData>("Scriptable Objects/Setting Data");
    }

    private void Start()
    {
        SetFps(Convert.ToInt32(_settingData.ListData[0]));
        PlayerPrefs.SetString(_settingData.ListSetting[0], _settingData.ListData[0]);
        PlayerPrefs.SetFloat(_settingData.ListSetting[1], float.Parse(_settingData.ListData[1]));
        PlayerPrefs.SetFloat(_settingData.ListSetting[2], float.Parse(_settingData.ListData[2]));
        PlayerPrefs.SetString(_settingData.ListSetting[3], _settingData.ListData[3]);
    }

    private static void SetFps(int frameRate)
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = frameRate;
    }

    public void OnVolumeUpdate(Component sender, object data)
    {
        var tmp = (VolumeData)data;

        PlayerPrefs.SetFloat(_settingData.ListSetting[1], tmp.MusicVolume);
        PlayerPrefs.SetFloat(_settingData.ListSetting[2], tmp.SoundVolume);

        _settingData.ListData[1] = tmp.MusicVolume.ToString("#.00");
        _settingData.ListData[2] = tmp.SoundVolume.ToString("#.00");

        onVolumeUpdated.Invoke(this, null);
    }
}