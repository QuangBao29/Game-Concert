using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreManager : SingletonMono<ScoreManager>
{
    [SerializeField] private TextMeshProUGUI scoreText = null;
    [SerializeField] private TextMeshProUGUI comboCountText = null;
    [SerializeField] private TextMeshProUGUI multiplierText = null;

    private float comboScore = 0;
    private float totalScore = 0;
    private int comboCount = 1;

    void Start()
    {
        comboScore = 0;
    }

    public void OnResponseNoteHit(Component sender, object data)
    {
        Debug.LogError("check hit");
        if (data is HitType hitData)
        {
            Debug.LogError("check perfect");
            if (hitData == HitType.Perfect)
            {
                comboCount += 1;
                int multiScore = GetMultiplier();
                float score = Define.PerfectScore * multiScore;
                //comboScore += score;
                totalScore += score;
                SetMultiplier(multiScore);
            }
            else if (hitData == HitType.Miss)
            {
                Debug.LogError("check miss");
                comboCount = 1;
                float score = Define.BaseScore;
                //comboScore = score;
                totalScore += score;
                SetMultiplier(1);
            }
            scoreText.text = totalScore.ToString();
            comboCountText.text = comboCount.ToString() + " HIT";
        }
    }

    private int GetMultiplier()
    {
        Debug.LogError("comboCount: " + comboCount);
        if (comboCount >= 1 && comboCount <= 5)
        {
            return 1;
        }
        else if (comboCount >= 6 && comboCount <= 10)
        {
            return 2;
        }
        else if (comboCount >= 11 && comboCount <= 20)
        {
            return 3;
        }
        else
        {
            return 5;
        }
    }

    public void SetMultiplier(int multiplier)
    {
        multiplierText.text = "x " + multiplier.ToString();
    }
}