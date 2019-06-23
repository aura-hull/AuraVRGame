using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using AuraHull.AuraVRGame;
using UnityEngine.UI;

[Serializable]
public class ScoreboardManager
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

    private string _filepath = Application.streamingAssetsPath + "/ScoreServer/scores.xml";
    public string Filepath { get; set; }

    [SerializeField] private int _scoreboardSize = 1000;
    private List<ScoreData> _scores;

    private ScoreboardManager()
    {
        _scores = new List<ScoreData>();
    }

    public void SaveScores()
    {
        Xml.Node rootNode = new Xml.Node("scores");

        for (int i = 0; i < _scores.Count; i++)
        {
            Xml.Node scoreNode;
            rootNode.AddChild("score", out scoreNode);
            
            scoreNode.AddChild("name", _scores[i].name);
            scoreNode.AddChild("value", _scores[i].score);
        }

        Xml.Write(_filepath, rootNode);
    }

    public void AddNewRecord(float score, string name = "Anon")
    {
        int insertAt = _scores.Count;
        for (int i = 0; i < _scores.Count; i++)
        {
            if (score > _scores[i].score)
            {
                insertAt = i;
                break;
            }
        }

        _scores.Insert(insertAt, new ScoreData(name, score));
        SaveScores();

        NetworkController.Instance.NotifyScoreSaved(score, name, insertAt);
    }

    public void SyncNewRecord(float score, string name, int insertAt)
    {
        _scores.Insert(insertAt, new ScoreData(name, score));
        SaveScores();
    }

    public void ClearScores()
    {
        _scores.Clear();
    }

    public void LoadScores()
    {
        _scores.Clear();

        XmlNodeList[] elementList = Xml.Read(_filepath, "score");
        if (elementList.Length == 0) return;

        foreach (XmlElement e in elementList[0])
        {
            string name = e.ChildNodes[0].InnerText;
            float score = float.Parse(e.ChildNodes[1].InnerText);

            _scores.Add(new ScoreData(name, score));
        }
    }

    public bool ScoresContainsName(string name)
    {
        string toLower = name.ToLower();

        foreach (ScoreData data in _scores)
        {
            if (data.name.ToLower() == toLower) return true;
        }

        return false;
    }
}

public struct ScoreData
{
    public bool valid;
    public string name;
    public float score;

    public ScoreData(string name, float score)
    {
        valid = true;
        this.name = name;
        this.score = score;
    }

    public override string ToString()
    {
        return ($"{name} ({score})");
    }
}
