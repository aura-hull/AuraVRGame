using System;
using System.Collections;
using System.Collections.Generic;
using AuraHull.AuraVRGame;
using Photon.Pun;
using UnityEngine;

[Serializable]
public class AuraGameManager
{
    private enum GameState
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

    private GameState _currentState;
    private PowerManager _powerManager;
    private ScoreManager _scoreManager;
    private ScoreboardManager _scoreboardManager;
    
    private float _playDurationLimit = 1440;
    private float _playDuration = 0;
    private float _dayCyclesPerPlaythrough = 1;

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
        _scoreManager = ScoreManager.Instance;
        //_scoreboardManager = ScoreboardManager.Instance;

        Setup();

        NetworkController.OnSyncManagers += Sync;
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

        _currentState = GameState.Tutorial;

        Sync(_powerManager.PowerProduced, _powerManager.PowerUsed, _powerManager.PowerStored, _playDuration, _scoreManager.Score);
    }

    // Update is called once per frame
    public void Execute()
    {
        if (PhotonNetwork.IsMasterClient)
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
    }

    private void ExecuteTutorial()
    {
        _currentState = GameState.Gameplay;
    }

    private void ExecuteGameplay()
    {
        _playDuration += Time.deltaTime;
        OnPlayDurationChanged?.Invoke();

        _powerManager.Update();

        if (_playDuration >= _playDurationLimit)
        {
            // Get final values
            float finalScore = _scoreManager.Score;
            float finalNetPower = _powerManager.PowerProduced - _powerManager.PowerUsed;

            // Game should end
            _currentState = GameState.GameOver;
            OnGameOver?.Invoke();
        }

        NetworkController.Instance.NotifySyncManagers(_powerManager.PowerProduced, _powerManager.PowerUsed, _powerManager.PowerStored, _playDuration, _scoreManager.Score);
    }

    private void ExecuteEnd()
    {
        AuraSceneManager.Instance.SceneReset();
    }
}
