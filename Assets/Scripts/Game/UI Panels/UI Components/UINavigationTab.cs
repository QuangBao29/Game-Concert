using TMPro;
using UI;
using EventData;
using UnityEngine;

public class UINavigationTab : BaseUI
{
    public TextMeshProUGUI usernameTxt;
    public TextMeshProUGUI coinAmountTxt;
    public TextMeshProUGUI gemAmountTxt;


    protected override void OnShow(UIParam param = null)
    {
        base.OnShow(param);
        var temp = PlayFabPlayerDataController.Instance.PlayerData;
        SetCoin(temp.Coin.ToString());
        SetGem(temp.Gem.ToString());
        SetUsername(temp.Username);
    }

    public void OnPlayClick()
    {
        PlaySoundOnClick();
        UIManager.Instance.HideUI(UIManager.Instance.currentUIIndex);
        UIManager.Instance.ShowUI(UIIndex.UIModeSelection);
    }

    public void OnHomeClick()
    {
        PlaySoundOnClick();
        UIManager.Instance.HideUI(UIManager.Instance.currentUIIndex);
        UIManager.Instance.ShowUI(UIIndex.UIMainMenu);
    }

    public void OnAvatarClick()
    {
        PlaySoundOnClick();
        var uiParam = new UIParam
        {
            Data = UIManager.Instance.currentUIIndex
        };
        UIManager.Instance.HideUI(UIManager.Instance.currentUIIndex);
        UIManager.Instance.HideUI(this);
        UIManager.Instance.ShowUI(UIIndex.UIAvatarSelection, uiParam);
    }

    private void SetCoin(string amount)
    {
        coinAmountTxt.SetText(amount);
    }

    private void SetGem(string amount)
    {
        gemAmountTxt.SetText(amount);
    }

    private void SetUsername(string username)
    {
        usernameTxt.SetText(username);
    }

    public void UpdateData(Component sender, object data)
    {
        var temp = (UserData)data;

        SetCoin(temp.Coin.ToString());
        SetGem(temp.Gem.ToString());
        SetUsername(temp.Username);
    }
}