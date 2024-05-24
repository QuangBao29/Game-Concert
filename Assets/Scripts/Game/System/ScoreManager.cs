using UnityEngine;
using TMPro;

public class ScoreManager : SingletonMono<ScoreManager>
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboCountText;

    public float totalScore;
    private int _comboCount;
    private int _highestComboCount;

    private int _highestMultiplier;
    private int _multiplier;
    private float _rewardMultiplier;
    private int _boosterMultiplier;

    public int coin;
    public int gem;

    private void Start()
    {
        totalScore = 0;
        _boosterMultiplier = 1;
        _highestMultiplier = 0;
        _highestComboCount = 0;
        _multiplier = 0;
        _comboCount = 0;
        coin = 0;
        gem = 0;
        _rewardMultiplier = 1.0f;
    }


    public void OnResponseNoteHit(Component sender, object data)
    {
        if (data is HitType hitData)
        {
            UpdateMultiplier();
            if (hitData == HitType.Perfect)
            {
                _comboCount += 1;
                if (_comboCount > _highestComboCount)
                {
                    _highestComboCount = _comboCount;
                }

                float score = Define.PerfectScore * _multiplier * _boosterMultiplier;
                totalScore += score;
                GenerateRandomGemAmount(1, 5);
                GenerateRandomCoinAmount(20, 50);
            }
            else if (hitData == HitType.Miss)
            {
                _comboCount = 0;
                float score = Define.NormalScore;
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
            <= 19 => 4,
            _ => 8
        };

        if (_multiplier > _highestMultiplier)
        {
            _highestMultiplier = _multiplier;
        }
    }

    private void GenerateRandomGemAmount(int min, int max)
    {
        gem += (int)(Random.Range(min, max) * _rewardMultiplier);
    }

    private void GenerateRandomCoinAmount(int min, int max)
    {
        coin += (int)(Random.Range(min, max) * _rewardMultiplier);
    }

    public void ProcessItem(Component sender, object data)
    {
        var tmp = (string)data;
        switch (tmp)
        {
            case "Booster":
                _boosterMultiplier = 2;
                break;
            case "Bronze Ticket":
                _rewardMultiplier = 1.5f;
                break;
            case "Diamond Ticket":
                _rewardMultiplier = 2.5f;
                break;
            case "Golden Ticket":
                _rewardMultiplier = 2.0f;
                break;
            case "Shield Combo":
                _comboCount = _highestComboCount;
                break;
            case "Shield Multiplier":
                _multiplier = _highestMultiplier;
                break;
            case "Silver Ticket":
                _rewardMultiplier = 1.75f;
                break;
        }
    }
}