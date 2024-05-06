using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class UIModeSelection : BaseUI
{
    public void OnSinglePlayerClick()
    {
        UIManager.Instance.HideUI(this);   
        UIManager.Instance.ShowUI(UIIndex.UISongSelection);   
    }
}
