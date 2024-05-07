using System;
using EventData;
using UnityEngine;

public class GameManager : SingletonMono<GameManager>
{
    public GameEvent onSongStart;
    public GameEvent onEndGame;

    private LevelData _levelData;
    private GameState _gameState;


    public void Awake()
    {
        _gameState = GameState.UI;
        _levelData = SceneManager.Instance.LevelData;
    }

    private void Start()
    {
        Invoke(nameof(StartGame), 3.0f);
    }

    public void StartGame()
    {
        onSongStart.Invoke(this, null);
        SongManager.Instance.ReadFromFile(_levelData.SongIndex);
        _gameState = GameState.Play;
    }

    public void EndGame()
    {
        onEndGame.Invoke(this, null);
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