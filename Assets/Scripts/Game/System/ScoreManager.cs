using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreManager : SingletonMono<ScoreManager>
{
    [SerializeField] private TextMeshProUGUI scoreText = null;
    [SerializeField] private TextMeshProUGUI comboText = null;

    private static int comboScore = 0;
    private static int totalScore = 0;
    private int comboCount = 0;

    void Start()
    {
        comboScore = 0;
    }

    public void OnNoteHit(Component sender, object data)
    {
        int multiScore = GetMultiplier();

        if (data is Tuple<HitType, bool> hitData)
        {
            if (hitData.Item1 == HitType.Perfect)
            {
                Debug.LogError("perfect hit");
                int score = Define.PerfectScore * multiScore;
            }
        }
    }

    private int GetMultiplier()
    {
        return 0;
    }

    public static void Hit()
    {
        comboScore += 1;
        //AudioManager.Instance.PlayHitSFX();
    }

    public static void Miss()
    {
        comboScore -= 1;
        //AudioManager.Instance.PlayMissSFX();
    }

    public void SetScore(int score)
    {
        scoreText.text = comboScore.ToString();
    }
}