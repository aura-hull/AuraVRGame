using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScoreboardManager
{
    #region Singleton
    private static ScoreboardManager _instance;
    public static ScoreboardManager Instance
    {
        get
        {
            if (_instance == null) _instance = new ScoreboardManager();
            return _instance;
        }
    }
    #endregion

    private string _filepath = "scores.txt";
    public string Filepath { get; set; }

    [SerializeField] private int _scoreboardSize = 1000;
    private List<ScoreData> _scores;

    private ScoreboardManager()
    {
        AuraGameManager.Instance.OnGameOver += SaveScores;
        _scores = new List<ScoreData>();
    }

    public void SaveScores()
    {
        try
        {
            List<string> lines = new List<string>();

            // Generate lines to write
            for (int i = 0; i < _scores.Count; i += 1)
            {
                if (_scores[i].valid)
                {
                    string line = _scores[i].ToString();
                    lines.Add(line);
                }
                else
                {
                    Debug.LogWarning("Attempted to save invalid score record \n" + 
                        "name: " + _scores[i].name + "\n" +
                        "score: " + _scores[i].score);
                }
            }

            // Concatinate into single string
            string toWrite = "";
            for (int i = 0; i < lines.Count; i += 1)
            {
                if (i != 0)
                    toWrite += Environment.NewLine;
                toWrite += lines[i];
            }

            File.WriteAllText(_filepath, toWrite);
        }
        catch
        {
            Debug.LogWarning("Failed to save scores at: " + _filepath);
        }
    }

    public void AddNewRecord(float score)
    {
        DateTime time = DateTime.Now;
        ScoreData newScoreData = new ScoreData("Not Set", score, time);

        if (_scores.Count == 0 || _scores.Count < _scoreboardSize)
        {
            _scores.Add(newScoreData);
        }
        else
        {
            int indexToBe = -1;

            // Find desired rank
            for (int i = 0; i < _scores.Count; i += 1)
            {
                if (score > _scores[i].score)
                {
                    indexToBe = i;
                    break;
                }
            }

            // Place at desired rank
            if (indexToBe != -1)
                _scores.Insert(indexToBe, newScoreData);

            // remove the excess records
            if (_scores.Count > _scoreboardSize)
                _scores.RemoveRange(_scoreboardSize, _scores.Count - _scoreboardSize);
        }
    }

    public void ClearScores()
    {
        _scores.Clear();
    }

    public void LoadScores()
    {
        List<ScoreData> scoresFromFile = new List<ScoreData>();

        try
        {
            using (StreamReader sr = new StreamReader(_filepath))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    ScoreData score = new ScoreData(line);

                    if (score.valid)
                        scoresFromFile.Add(score);
                }
            }
        }
        catch
        {
            Debug.LogWarning("Failed to load scores from: " + _filepath);
        }

        _scores = scoresFromFile;
    }
}

public struct ScoreData
{
    public bool valid;
    public string name;
    public float score;
    public DateTime time;
    
    public ScoreData(string name, float score, DateTime time)
    {
        valid = true;
        this.name = name;
        this.score = score;
        this.time = time;
    }

    public ScoreData(string dataLine)
    {
        string[] parts = dataLine.Split(new char[] { '-' }, 3);

        if (parts.Length == 3)
        {
            valid = true;
            name = parts[0];
            float.TryParse(parts[1], out score);
            DateTime.TryParse(parts[2], out time);
        }
        else
        {
            valid = false;
            name = dataLine;
            score = 0;
            time = DateTime.Now;
        }
    }

    public override string ToString()
    {
        return (name + "-" + score + "-" + time);
    }
}
