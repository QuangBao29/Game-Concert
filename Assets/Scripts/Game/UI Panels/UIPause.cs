using UI;

public class UIPause : BaseUI
{
    public GameEvent onVolumeChange;
    public GameEvent onResumeClick;
    public GameEvent onRetryClick;
    public GameEvent onQuitClick;

    public void OnVolumeChange()
    {
        onVolumeChange.Invoke(this, null);
    }

    public void OnResumeClick()
    {
        onResumeClick.Invoke(this, null);
        UIManager.Instance.HideUI(index);
        UIManager.Instance.ShowUI(UIIndex.UIHud);
    }

    public void OnRetryClick()
    {
        onRetryClick.Invoke(this, null);
    }

    public void OnQuitClick()
    {
        onQuitClick.Invoke(this, null);
    }
}