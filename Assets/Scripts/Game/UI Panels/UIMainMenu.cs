using UI;
using UnityEngine;

public class UIMainMenu : BaseUI
{
    public void OnShopClick()
    {
        PlaySoundOnClick();
        UIManager.Instance.HideUI(index);
        UIManager.Instance.ShowUI(UIIndex.UIShop);
    }

    public void OnInventoryClick()
    {
        PlaySoundOnClick();
        UIManager.Instance.HideUI(index);
        UIManager.Instance.ShowUI(UIIndex.UIInventory);
    }

    public void OnSettingClick()
    {
        PlaySoundOnClick();
        UIManager.Instance.HideUI(index);
        UIManager.Instance.HideUI(UIIndex.UINavigationTab);
        UIManager.Instance.ShowUI(UIIndex.UISetting);
    }

    public void OnEditConcertClick() {
        PlaySoundOnClick();
        UIManager.Instance.HideUI(index);
        UIManager.Instance.HideUI(UIIndex.UINavigationTab);
        UIManager.Instance.ShowUI(UIIndex.UIConcertEditor);

    }

    public void OnEditLevelClick() {
        PlaySoundOnClick();
        UIManager.Instance.HideUI(index);
        UIManager.Instance.HideUI(UIIndex.UINavigationTab);
        UIManager.Instance.ShowUI(UIIndex.UILevelEditor);
    }

    public void OnExitClick()
    {
        Application.Quit();
    }

    public void OnCharacterSelectionClick()
    {
        PlaySoundOnClick();
        UIManager.Instance.HideUI(index);
        UIManager.Instance.ShowUI(UIIndex.UICharacterSelection);
    }
}