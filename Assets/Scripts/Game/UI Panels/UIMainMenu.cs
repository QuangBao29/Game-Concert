using UI;
using UnityEngine;

public class UIMainMenu : BaseUI
{
    public void OnShopClick()
    {
        UIManager.Instance.HideUI(index);
        UIManager.Instance.ShowUI(UIIndex.UIShop);
    }

    public void OnInventoryClick()
    {
        UIManager.Instance.HideUI(index);
        UIManager.Instance.ShowUI(UIIndex.UIInventory);
    }

    public void OnSettingClick()
    {
        UIManager.Instance.HideUI(index);
        UIManager.Instance.HideUI(UIIndex.UINavigationTab);
        UIManager.Instance.ShowUI(UIIndex.UISetting);
    }


    public void OnExitClick()
    {
        Application.Quit();
    }

    public void OnCharacterSelectionClick()
    {
        UIManager.Instance.HideUI(index);
        UIManager.Instance.ShowUI(UIIndex.UICharacterSelection);
    }
}