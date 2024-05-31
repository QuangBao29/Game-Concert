using System;
using System.Collections;
using System.Collections.Generic;
using EventData;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class UIVictory : BaseUI
{
    public GameEvent onRetryClick;
    public GameEvent onNewGameClick;

    public TextMeshProUGUI score;
    public TextMeshProUGUI rank;

    public TextMeshProUGUI coinAmount;
    public TextMeshProUGUI gemAmount;

    public void OnRetryClick()
    {
        PlaySoundOnClick();
        onRetryClick.Invoke(this, null);
    }

    public void OnNewGameClick()
    {
        PlaySoundOnClick();
        onNewGameClick.Invoke(this, null);
    }


    protected override void OnShow(UIParam param = null)
    {
        base.OnShow(param);
        if (param != null)
        {

            var temp = (EndLevelData)param.Data;
            if (temp.IsCustom)
            {
                rank.SetText("Unranked");

            }
            else
            {
                rank.SetText(PlayFabGameDataController.Instance.PlayerRank.ToString());
            }

            score.SetText(temp.Score.ToString());
            coinAmount.SetText(temp.Coin.ToString());
            gemAmount.SetText(temp.Gem.ToString());
        }
    }
}