using UI;
using UnityEngine.UI;
using UnityEngine;
using EventData;

public class UIPause : BaseUI
{
    public GameEvent onVolumeChange;
    public GameEvent onResumeClick;
    public GameEvent onRetryClick;
    public GameEvent onQuitClick;

    public Slider musicVolumeSlider;
    public Slider soundVolumeSlider;


    protected override void OnShow(UIParam param = null)
    {
        base.OnShow(param);

        var musicVolumeValue = PlayerPrefs.GetFloat("Music Volume");
        var soundVolumeValue = PlayerPrefs.GetFloat("Sound Volume");

        musicVolumeSlider.value = musicVolumeValue;
        soundVolumeSlider.value = soundVolumeValue;
    }

    public void OnVolumeChange()
    {
        onVolumeChange.Invoke(this, new VolumeData
        {
            SoundVolume = soundVolumeSlider.value,
            MusicVolume = soundVolumeSlider.value
        });
    }

    public void OnResumeClick()
    {
        onResumeClick.Invoke(this, null);
        UIManager.Instance.HideUI(index);
        UIManager.Instance.ShowUI(UIIndex.UIHud);
    }

    public void OnRetryClick()
    {
        Time.timeScale = 1;
        onRetryClick.Invoke(this, null);
        UIManager.Instance.HideUI(index);
        UIManager.Instance.ShowUI(UIIndex.UIHud);
    }

    public void OnQuitClick()
    {
        Time.timeScale = 1;
        onQuitClick.Invoke(this, null);
    }
}