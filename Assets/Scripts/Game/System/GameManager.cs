using System;
using EventData;
using UnityEngine;

public class GameManager : SingletonMono<GameManager>
{
    public GameEvent onSongStart;
    public GameEvent onEndGame;
    public GameEvent updateLeaderBoard;
    public GameEvent updateReward;

    private LevelData _levelData;
    private UserLevelData _userLevelData;
    private bool _isCustom = false;
    private GameState _gameState;


    public void Awake()
    {
        _gameState = GameState.UI;
        if (SceneManager.Instance.LevelData.SongIndex != -1)
        {
            _levelData = SceneManager.Instance.LevelData;
            _isCustom = false;
        }
        else
        {
            _userLevelData = SceneManager.Instance.UserLevelData;
            _isCustom = true;
        }
    }

    private void Start()
    {
        Invoke(nameof(StartGame), 3.0f);
    }

    public void StartGame()
    {
        if (_isCustom)
        {
            SongManager.Instance.ReadFromUserFile(_userLevelData.MIDIPath);
        }
        else
        {

            SongManager.Instance.ReadFromFile(_levelData.SongName);
        }
        _gameState = GameState.Play;
        onSongStart.Invoke(this, null);
    }

    private void EndGame()
    {
        onEndGame.Invoke(this, new EndLevelData
        {
            Coin = ScoreManager.Instance.coin,
            Gem = ScoreManager.Instance.gem,
            Score = (int)ScoreManager.Instance.totalScore,
            IsCustom = _isCustom
        });
    }

    public void ProcessEndSong(Component sender, object data)
    {
        if (!_isCustom)
        {

            updateLeaderBoard.Invoke(this, new UpdateLeaderBoardReqInfo
            {
                Name = _levelData.SongName,
                Score = (int)ScoreManager.Instance.totalScore,
                SuccessCallback = EndGame
            });

            updateReward.Invoke(this, new RewardData
            {
                CoinKey = "CN",
                CoinAmount = ScoreManager.Instance.coin,
                GemKey = "GM",
                GemAmount = ScoreManager.Instance.gem
            });
        }
        else {
            updateReward.Invoke(this, new RewardData
            {
                CoinKey = "CN",
                CoinAmount = ScoreManager.Instance.coin,
                GemKey = "GM",
                GemAmount = ScoreManager.Instance.gem
            });
            EndGame();
        }
    }


    public void PauseGame(Component sender, object data)
    {
        Time.timeScale = 0;
    }

    public void ResumeGame(Component sender, object data)
    {
        Time.timeScale = 1;
    }

    private void ProcessGameplay()
    {
    }

    private void Update()
    {
        switch (_gameState)
        {
            case GameState.UI:
                break;
            case GameState.Play:
                ProcessGameplay();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

public enum GameState
{
    UI,
    Play
}