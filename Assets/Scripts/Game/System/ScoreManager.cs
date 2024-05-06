using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : PersistentManager<ScoreManager>
{
    [SerializeField]
    private TextMeshProUGUI scoreText = null;
    [SerializeField]
    private TextMeshProUGUI _comboText = null;

    private static int _comboScore;

    void Start()
    {
        _comboScore = 0;
    }
    public static void Hit()
    {
        _comboScore += 1;
        //AudioManager.Instance.PlayHitSFX();
    }
    public static void Miss()
    {
        _comboScore -= 1;
        //AudioManager.Instance.PlayMissSFX();
    }
    public void SetScore(int score)
    {
        scoreText.text = _comboScore.ToString();
    }
}
