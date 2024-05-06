using System;
using EventData;
using UnityEngine;

public class GameManager : SingletonMono<GameManager>
{
    public GameEvent onSongStart;
    public GameEvent onPauseClick;

    private LevelData _levelData;
    private GameState _gameState;


    public void Awake()
    {
        _gameState = GameState.UI;
        _levelData = SceneManager.Instance.levelData;
    }

    private void Start()
    {
        Invoke(nameof(StartGame), 3.0f);
    }


    public void StartGame()
    {
        onSongStart.Invoke(this, null);
        SongManager.Instance.ReadFromFile(_levelData.SongIndex);
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

    private void ProcessGameplay()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            onPauseClick.Invoke(this, null);
            Time.timeScale = 0;
        }
    }
}

public enum GameState
{
    UI,
    Play
}