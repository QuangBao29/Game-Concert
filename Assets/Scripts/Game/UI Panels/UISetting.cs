using EventData;
using UnityEngine.UI;
using UI;
using UnityEngine;

public class UISetting : BaseUI
{
    public GameEvent onVolumeChange;
    public GameEvent onLogoutClick;

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

    public void OnBackClick()
    {
        PlaySoundOnClick();
        UIManager.Instance.HideUI(this);
        UIManager.Instance.ShowUI(UIIndex.UINavigationTab);
        UIManager.Instance.ShowUI(UIIndex.UIMainMenu);
    }

    public void OnLogoutClick()
    {
        PlaySoundOnClick();
        onLogoutClick.Invoke(this, null);
        UIManager.Instance.HideUI(this);
        UIManager.Instance.ShowUI(UIIndex.UIAuthentication);
    }

    public void OnVolumeChange()
    {
        onVolumeChange.Invoke(this, new VolumeData
        {
            SoundVolume = soundVolumeSlider.value,
            MusicVolume = soundVolumeSlider.value
        });
    }
}