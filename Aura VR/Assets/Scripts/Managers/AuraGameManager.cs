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

    public Action OnTutorialStarted;
    public Action OnGameplayStarted;
    public Action OnGameOver;
    public Action OnPlayDurationChanged;
    
    private PowerManager _powerManager;
    private UpgradeManager _upgradeManager;
    private ScoreManager _scoreManager;
    private ScoreboardManager _scoreboardManager;
    
    private float _playDurationLimit = 10;
    private float _playDuration = 0;

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

    private AuraGameManager()
    {
        _powerManager = PowerManager.Instance;
        _upgradeManager = UpgradeManager.Instance;
        _scoreManager = ScoreManager.Instance;
        _scoreboardManager = ScoreboardManager.Instance;

        OnTutorialStarted += _upgradeManager.DisableUpgrades;
        OnGameplayStarted += _upgradeManager.EnableUpgrades;
        OnGameOver += _upgradeManager.DisableUpgrades;

        Setup();

        NetworkController.OnSyncManagers += Sync;
        NetworkController.OnUpgradedOrDowngraded += _upgradeManager.SyncUpgradeState;
        NetworkController.OnScoreSaved += _scoreboardManager.SyncNewRecord;
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
    
    private void ExecuteGameplay()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        _playDuration += Time.deltaTime;
        OnPlayDurationChanged?.Invoke();

        _powerManager.Update();

        if (_playDuration >= _playDurationLimit)
        {
            // Game should end
            SetState(GameState.GameOver);
        }

        _scoreManager.Score = _powerManager.PowerStored;

        NetworkController.Instance.NotifySyncManagers(_powerManager.PowerProduced, _powerManager.PowerUsed, _powerManager.PowerStored, _playDuration, _scoreManager.Score);
    }

    private void ExecuteEnd()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetScoreText(_scoreManager.ScoreInt);
            gameOverScreen.gameObject.SetActive(true);
        }
        else
        {
            SaveAndReset("Anon", _scoreManager.ScoreInt);
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
                OnTutorialStarted?.Invoke();
                break;

            case GameState.Gameplay:
                ResetManager();
                OnGameplayStarted?.Invoke();
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
        _scoreboardManager.AddNewRecord(_scoreManager.ScoreInt, name); // Saves automatically
        AuraSceneManager.Instance.SceneReset();
    }
}
