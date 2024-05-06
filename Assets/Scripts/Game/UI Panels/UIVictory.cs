using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIVictory : BaseUI
{
    public GameEvent onRetryClick;
    public GameEvent onNewGameClick;

    public TextMeshProUGUI score;
    public TextMeshProUGUI rank;

    public TextMeshProUGUI songTitle;
    public TextMeshProUGUI songArtist;
    public Image songCover;

    public void OnRetryClick()
    {
        onRetryClick.Invoke(this, null);
    }
    
    public void OnNewGameClick()
    {
        onNewGameClick.Invoke(this, null);
    }

    public void UpdateSongData(Component sender, object data)
    {
        //
    }

    public void UpdateData(Component sender, object data)
    {
        // score.SetText();
        // rank.SetText();
    }
}
