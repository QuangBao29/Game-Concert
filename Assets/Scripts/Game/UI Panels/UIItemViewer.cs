using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemViewer : BaseUI
{
    public GameEvent onBuyClick;

    public void OnBuyClick()
    {
        onBuyClick.Invoke(this, null);
    }
}