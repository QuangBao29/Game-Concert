using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemViewer : BaseUI
{
    public GameEvent onBuyClick;

    public void OnBuyClick()
    {
        PlaySoundOnClick();        PlaySoundOnClick();

        onBuyClick.Invoke(this, null);
    }
}