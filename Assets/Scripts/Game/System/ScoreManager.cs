using UnityEngine;
using TMPro;

public class ScoreManager : SingletonMono<ScoreManager>
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboCountText;

    public float totalScore;
    private int _comboCount;

    private int _highestMultiplier;
    private int _multiplier;

    public int coin;
    public int gem;

    private void Start()
    {
        totalScore = 0;
        _highestMultiplier = 0;
        _multiplier = 0;
        _comboCount = 0;
        coin = 0;
        gem = 0;
    }


    public void OnResponseNoteHit(Component sender, object data)
    {
        if (data is HitType hitData)
        {
            UpdateMultiplier();
            if (hitData == HitType.Perfect)
            {
                _comboCount += 1;
                float score = Define.PerfectScore * _multiplier;
                totalScore += score;
                GenerateRandomGemAmount(1, 5);
                GenerateRandomCoinAmount(20, 50);
            }
            else if (hitData == HitType.Miss)
            {
                _comboCount = 0;
                float score = Define.BaseScore;
                totalScore += score;
                GenerateRandomCoinAmount(5, 10);
            }

            scoreText.text = ((int)totalScore).ToString();
            comboCountText.text = _comboCount + " HIT";
        }
    }

    private void UpdateMultiplier()
    {
        _multiplier = _comboCount switch
        {
            >= 0 and <= 4 => 1,
            <= 9 => 2,
            <= 19 => 3,
            _ => 5
        };

        if (_multiplier > _highestMultiplier)
        {
            _highestMultiplier = _multiplier;
        }
    }

    private void GenerateRandomGemAmount(int min, int max)
    {
        gem += Random.Range(min, max);
    }

    private void GenerateRandomCoinAmount(int min, int max)
    {
        coin += Random.Range(min, max);
    }

    public void ProcessItem(Component sender, object data)
    {
        var tmp = (string)data;
        switch (tmp)
        {
            case "Booster":
                break;
            case "Bronze Ticket":
                break;
            case "Diamond Ticket":
                break;
            case "Golden Ticket":
                break;
            case "Shield Combo":
                break;
            case "Shield Multiplier":
                break;
            case "Silver Ticket":
                break;
        }
    }
}