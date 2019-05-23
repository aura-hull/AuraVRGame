using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreboardManager : MonoBehaviour
{
    #region Singleton
    private static ScoreboardManager _instance;

    public static ScoreboardManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ScoreboardManager();
            return _instance;
        }
    }
    #endregion

    private string _filepath = "Data/Scores.txt";
    public string Filepath { get; set; }

    private int _scoreboardSize = 100;

    private ScoreboardManager()
    {
        GameManager.Instance.OnGameOver += SaveScoresToFile;
    }


    private void SaveScoresToFile()
    {
        List<ScoreData> scores = LoadScores();

        // Get score of the current game
        ScoreManager scoreManager = ScoreManager.Instance;
        float score = scoreManager.Score;

        for (int i = 0; i < scores.Count; i += 1)
        {
            if (score < scores[i].score)
            {
                // Create record with current score
                DateTime time = DateTime.Now;
                ScoreData newScoreData = new ScoreData("Not Set", score, time);

                // place in list
                scores.Insert(i, newScoreData);

                break;
            }
        }

        if (scores.Count > _scoreboardSize)
        {
            // remove the excess records
            scores.RemoveRange(_scoreboardSize, scores.Count - _scoreboardSize);
        }

        try
        {
            using (StreamWriter sw = new StreamWriter(_filepath))
            {
                for (int i = 0; i < scores.Count; i += 1)
                {
                    if (scores[i].valid)
                    {
                        string line = scores[i].ToString();

                        sw.WriteLine(line);
                    }
                }

                sw.Close();
            }
        }
        catch
        {
            Debug.LogWarning("Failed to save scores at: " + _filepath);
        }
    }

    private List<ScoreData> LoadScores()
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

        return scoresFromFile;
    }
}

struct ScoreData
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
        string[] parts = dataLine.Split(' ');

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
            name = "name";
            score = 0;
            time = DateTime.Now;
        }
    }

    public override string ToString()
    {
        return "";
    }
}
