using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager
{
    #region Singleton
    private static ScoreManager _instance;
    public static ScoreManager Instance
    {
        get
        {
            if (_instance == null) _instance = new ScoreManager();
            return _instance;
        }
    }
    #endregion

    private ScoreManager() { }

    private float _score;
    public Action OnScoreChange;

    public float Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            OnScoreChange?.Invoke();
        }
    }

    public void ResetScore()
    {
        Score = 0;
    }
    public void AddScore(float toAdd)
    {
        Score += toAdd;
    }
}
