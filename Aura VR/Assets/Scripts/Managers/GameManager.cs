using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Tutorial,
    Gameplay,
    EndScreen
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    public GameState CurrentGameState;
    public Action OnGameOver;

    private PowerManager _powerManager;
    private ScoreManager _scoreManager;
    private PoolManager _poolManager;
    private ScoreboardManager _scoreboardManager;

    [SerializeField]
    private float _playDurationLimit = 30;
    private float _playDuration = 0;
    public Action<float, float> onPlayDurationChanged;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentGameState = GameState.Gameplay;

        _powerManager = PowerManager.Instance;
        _scoreManager = ScoreManager.Instance;
        _poolManager = PoolManager.Instance;
        _scoreboardManager = ScoreboardManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentGameState)
        {
            case GameState.Tutorial:
                UpdateTutorial();
                break;
            case GameState.Gameplay:
                UpdateGameplay();
                break;
            case GameState.EndScreen:
                UpdateEndScreen();
                break;
        }
    }

    private void UpdateTutorial()
    {

    }

    private void UpdateGameplay()
    {
        _playDuration += Time.deltaTime;
        onPlayDurationChanged?.Invoke(_playDuration, _playDurationLimit);

        if (_playDuration >= _playDurationLimit)
        {
            // Get final values
            float finalScore = _scoreManager.Score;
            float finalNetPower = _powerManager.PowerProduced - _powerManager.PowerUsed;

            // Game should end

            OnGameOver?.Invoke();
        }
    }

    private void UpdateEndScreen()
    {

    }
}
