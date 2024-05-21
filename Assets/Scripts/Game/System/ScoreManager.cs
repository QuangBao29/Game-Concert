using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreManager : SingletonMono<ScoreManager>
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboCountText;

    private float _comboScore = 0;
    public float totalScore = 0;
    private int _comboCount = 0;

    private int _coin;
    private int _gem;


    public void OnResponseNoteHit(Component sender, object data)
    {
        if (data is HitType hitData)
        {
            if (hitData == HitType.Perfect)
            {
                _comboCount += 1;
                var multiplier = GetMultiplier();
                float score = Define.PerfectScore * multiplier;
                totalScore += score;
            }
            else if (hitData == HitType.Miss)
            {
                _comboCount = 0;
                float score = Define.BaseScore;
                totalScore += score;
            }

            scoreText.text = ((int)totalScore).ToString();
            comboCountText.text = _comboCount + " HIT";
        }
    }

    private int GetMultiplier()
    {
        if (_comboCount is >= 0 and <= 4)
        {
            return 1;
        }

        if (_comboCount is >= 5 and <= 9)
        {
            return 2;
        }

        if (_comboCount is >= 10 and <= 19)
        {
            return 3;
        }

        return 5;
    }

    public void ProcessItem(Component sender, object data)
    {
    }
}