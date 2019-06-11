using System;
using System.Collections;
using System.Collections.Generic;
using AuraHull.AuraVRGame;
using Photon.Pun;
using UnityEngine;

public class AuraGameManager
{
    private enum GameState
    {
        Tutorial,
        Gameplay,
        EndScreen
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
    public Action<float, float> OnPlayDurationChanged;

    private GameState _currentState;
    private PowerManager _powerManager;
    private ScoreManager _scoreManager;
    private ScoreboardManager _scoreboardManager;
    
    private float _playDurationLimit = 30;
    private float _playDuration = 0;

    private AuraGameManager()
    {
        _powerManager = PowerManager.Instance;
        _powerManager.PowerProduced = 0;
        _powerManager.PowerUsed = 0;

        _scoreManager = ScoreManager.Instance;
        _scoreManager.Score = 0;
        //_scoreboardManager = ScoreboardManager.Instance;

        _currentState = GameState.Gameplay;

        NetworkController.OnSyncManagers += Sync;
    }

    void Sync(float playDuration, float score, float powerProduced, float powerUsed)
    {
        if (PhotonNetwork.IsMasterClient) return;

        _playDuration = playDuration;
        OnPlayDurationChanged?.Invoke(_playDuration, _playDurationLimit);

        _scoreManager.Score = score;
        _powerManager.PowerProduced = powerProduced;
        _powerManager.PowerUsed = powerUsed;
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
                case GameState.EndScreen:
                    ExecuteEnd();
                    break;
            }
        }
    }

    private void ExecuteTutorial()
    {

    }

    private void ExecuteGameplay()
    {
        _playDuration += Time.deltaTime;
        OnPlayDurationChanged?.Invoke(_playDuration, _playDurationLimit);

        if (_playDuration >= _playDurationLimit)
        {
            // Get final values
            float finalScore = _scoreManager.Score;
            float finalNetPower = _powerManager.PowerProduced - _powerManager.PowerUsed;

            // Game should end
            _currentState = GameState.EndScreen;
            OnGameOver?.Invoke();
        }

        NetworkController.Instance.NotifySyncManagers(_playDuration, _scoreManager.Score, _powerManager.PowerProduced, _powerManager.PowerUsed);
    }

    private void ExecuteEnd()
    {

    }
}
