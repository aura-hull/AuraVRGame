using System;
using System.Collections;
using System.Collections.Generic;
using AuraHull.AuraVRGame;
using Photon.Pun;
using UnityEngine;

[Serializable]
public class AuraGameManager
{
    public enum GameState
    {
        Tutorial,
        Gameplay,
        GameOver
    }

    private static AuraGameManager _instance;
    public static AuraGameManager Instance
    {
        get
        {
            if (_instance == null) _instance = new AuraGameManager();
            return _instance;
        }
    }
    
    public Action OnGameOver;
    public Action OnPlayDurationChanged;
    
    private PowerManager _powerManager;
    private UpgradeManager _upgradeManager;
    private ScoreManager _scoreManager;
    private ScoreboardManager _scoreboardManager;
    
    private float _playDurationLimit = 600;
    private float _playDuration = 0;
    private float _dayCyclesPerPlaythrough = 1;

    private GameState _currentState;

    private GameOverScreen _gameOverScreen = null;
    public GameOverScreen gameOverScreen
    {
        get { return _gameOverScreen; }
        set
        {
            _gameOverScreen = value;
            _gameOverScreen.OnPlayerDataConfirmed += SaveAndReset;
            _gameOverScreen.ResetAndHide();
        }
    }

    public float TimeRemaining
    {
        get { return Mathf.Round(_playDurationLimit - _playDuration); }
    }

    public float PlayDurationLimit
    {
        get { return _playDurationLimit; }
    }

    public float DayCyclesPerPlaythrough
    {
        get { return _dayCyclesPerPlaythrough; }
    }

    private AuraGameManager()
    {
        _powerManager = PowerManager.Instance;
        _upgradeManager = UpgradeManager.Instance;
        _scoreManager = ScoreManager.Instance;
        _scoreboardManager = ScoreboardManager.Instance;

        Setup();

        NetworkController.OnSyncManagers += Sync;
        NetworkController.OnUpgradedOrDowngraded += _upgradeManager.SyncUpgradeState;
        AuraSceneManager.Instance.SubscribeOnSceneReset(Setup);
    }

    void Sync(float powerProduced, float powerUsed, float powerStored, float playDuration, float score)
    {
        if (PhotonNetwork.IsMasterClient) return;
        
        _powerManager.PowerProduced = powerProduced;
        _powerManager.PowerUsed = powerUsed;
        _powerManager.PowerStored = powerStored;

        _playDuration = playDuration;
        OnPlayDurationChanged?.Invoke();

        _scoreManager.Score = score;
    }

    void Setup()
    {
        _playDuration = 0;

        _powerManager.depletePowerTime = 86400 / _playDurationLimit / _dayCyclesPerPlaythrough;
        _powerManager.PowerProduced = 0;
        _powerManager.PowerUsed = 0;
        _powerManager.PowerStored = 600;

        _scoreManager.Score = 0;

        _scoreboardManager.LoadScores();

        SetState(GameState.Tutorial);

        Sync(_powerManager.PowerProduced, _powerManager.PowerUsed, _powerManager.PowerStored, _playDuration, _scoreManager.Score);
    }

    // Update is called once per frame
    public void Execute()
    {
        switch (_currentState)
        {
            case GameState.Tutorial:
                ExecuteTutorial();
                break;
            case GameState.Gameplay:
                ExecuteGameplay();
                break;
            case GameState.GameOver:
                ExecuteEnd();
                break;
        }
    }

    private void ExecuteTutorial()
    {
        if (!TutorialManager.Instance.isRunning)
        {
            SetState(GameState.Tutorial);
        }
    }

    float temp = 0.0f;
    private void ExecuteGameplay()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        _playDuration += Time.deltaTime;
        OnPlayDurationChanged?.Invoke();

        _powerManager.Update();

        // REMOVE THIS: UPGRADES EVERY 20 SECONDS!!
        temp += Time.deltaTime;
        if (temp > 20.0f)
        {
            _upgradeManager.Upgrade();
            temp = 0.0f;
        }

        if (_playDuration >= _playDurationLimit)
        {
            // Get final values
            float finalScore = _scoreManager.Score;
            float finalNetPower = _powerManager.PowerProduced - _powerManager.PowerUsed;

            // Game should end
            SetState(GameState.GameOver);
        }

        _scoreManager.Score += _powerManager.PowerProduced;

        NetworkController.Instance.NotifySyncManagers(_powerManager.PowerProduced, _powerManager.PowerUsed, _powerManager.PowerStored, _playDuration, _scoreManager.Score);
    }

    private void ExecuteEnd()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetScoreText(_scoreManager.Score);
            gameOverScreen.gameObject.SetActive(true);
        }
        else
        {
            SaveAndReset("Anon", _scoreManager.Score);
        }
    }

    public void SetState(GameState newState)
    {
        _currentState = newState;

        switch (_currentState)
        {
            case GameState.Tutorial:
                ResetManager();
                TutorialManager.Instance.StartTutorial();
                break;

            case GameState.Gameplay:
                ResetManager();
                break;

            case GameState.GameOver:
                OnGameOver?.Invoke();
                break;
        }
    }

    private void ResetManager()
    {
        _playDuration = 0;
        OnPlayDurationChanged?.Invoke();
    }

    private void SaveAndReset(string name, float score)
    {
        _scoreboardManager.AddNewRecord(_scoreManager.Score, name);
        _scoreboardManager.SaveScores();

        AuraSceneManager.Instance.SceneReset();
    }
}
