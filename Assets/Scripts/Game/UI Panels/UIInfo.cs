using TMPro;
using UI;

public class UIInfo : BaseUI
{
    public TextMeshProUGUI displayText;

    protected override void OnShow(UIParam param = null)
    {
        base.OnShow(param);
        if (param != null)
        {
            var tmp = (string)param.Data;
            displayText.SetText(tmp);
        }
    }

    public void OnHideClick()
    {
        UIManager.Instance.HideUI(this);
    }
}