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
    public static GameManager Instance = null;

    [SerializeField]
    public GameState CurrentGameState;

    private PowerManager _powerManager;
    private ScoreManager _scoreManager;
    private PoolManager _poolManager;

    [SerializeField]
    private float _playDurationLimit = 30;
    private float _playDuration = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        CurrentGameState = GameState.Tutorial;

        _powerManager = PowerManager.Instance;
        _scoreManager = ScoreManager.Instance;
        _poolManager = PoolManager.Instance;
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

        if (_playDuration >= _playDurationLimit)
        {
            // Get final values
            float finalScore = _scoreManager.Score;
            float finalNetPower = _powerManager.PowerProduced - _powerManager.PowerUsed;

            // Game should end
        }
    }

    private void UpdateEndScreen()
    {

    }
}
