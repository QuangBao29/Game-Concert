using UI;

public class UIConcertEditor : BaseUI
{
    public void OnBackClick()
    {
        PlaySoundOnClick();
        UIManager.Instance.HideUI(this);
        UIManager.Instance.ShowUI(UIIndex.UINavigationTab);
        UIManager.Instance.ShowUI(UIIndex.UIMainMenu);
    }
}
