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
            if (_instance == null)
            {
                _instance = new ScoreManager();
            }
            return _instance;
        }
    }
    #endregion

    private ScoreManager()
    {

    }

    private float _score;
    public Action<float> OnScoreChange;

    public float Score
    {
        get
        {
            return _score;
        }
        private set
        {
            _score = value;
            OnScoreChange?.Invoke(_score);
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
