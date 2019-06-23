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
        Null,
        Waiting,
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

    public Action OnStateWaiting;
    public Action OnGameplayStarted;
    public Action OnGameOver;
    public Action OnPlayDurationChanged;
    
    private PowerManager _powerManager;
    private UpgradeManager _upgradeManager;
    private ScoreManager _scoreManager;
    private ScoreboardManager _scoreboardManager;
    private TutorialManager _tutorialManager;

    private float _playDurationLimit = 600;
    private float _playDuration = 0;

    private GameState _currentState;
    private GameState _returnToState;

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
        _tutorialManager = TutorialManager.Instance;

        Setup();

        NetworkController.OnGameStateChanged += SyncState;
        NetworkController.OnSyncManagers += Sync;
        NetworkController.OnUpgradedOrDowngraded += _upgradeManager.SyncUpgradeState;
        NetworkController.OnScoreSaved += _scoreboardManager.SyncNewRecord;
        AuraSceneManager.Instance.SubscribeOnSceneReset(Setup);
    }

    void SyncState(GameState state)
    {
        if (PhotonNetwork.IsMasterClient) return;
        SetState(state);
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

        _returnToState = GameState.Null;
        SetState(GameState.Waiting);
        ResetManager();

        Sync(_powerManager.PowerProduced, _powerManager.PowerUsed, _powerManager.PowerStored, _playDuration, _scoreManager.Score);
    }

    // Update is called once per frame
    public void Execute()
    {
        if (NetworkController.ConnectedPlayers < GameModel.Instance.RequiredClients && _currentState != GameState.Waiting)
        {
            _returnToState = _currentState;
            SetState(GameState.Waiting);
        }

        switch (_currentState)
        {
            case GameState.Waiting:
                ExecuteWaiting();
                break;

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

    private void ExecuteWaiting()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (NetworkController.ConnectedPlayers >= GameModel.Instance.RequiredClients)
        {
            if (_returnToState != GameState.Null)
            {
                // This happens if a client disconnected.
                SetState(_returnToState);
                _returnToState = GameState.Null;
            }
            else
            {
                // This happens at the start of a playthrough.
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SetState(GameState.Tutorial);
                }
            }
        }
    }

    private void ExecuteTutorial()
    {
        GameModel.Instance.SpawnParts();
    }
    
    private void ExecuteGameplay()
    {
        GameModel.Instance.SpawnParts();
        GameModel.Instance.IssueUpgrades();

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
        if (!PhotonNetwork.IsMasterClient) return;

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

        if (PhotonNetwork.IsMasterClient)
        {
            NetworkController.Instance.NotifyGameStateChanged(_currentState);
        }

        switch (_currentState)
        {
            case GameState.Waiting:
                OnStateWaiting?.Invoke();
                break;

            case GameState.Tutorial:
                TutorialManager.Instance.StartTutorial();
                break;

            case GameState.Gameplay:
                OnGameplayStarted?.Invoke();
                break;

            case GameState.GameOver:
                ResetManager();
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
