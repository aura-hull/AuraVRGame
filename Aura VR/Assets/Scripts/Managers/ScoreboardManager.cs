using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
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
        try
        {
            if (!File.Exists(_filepath))
            {
                string path = Path.Combine(Application.streamingAssetsPath, "ScoreServer");
                Directory.CreateDirectory(path);
            }

            XmlDocument doc = new XmlDocument();

            XmlNode rootNode = doc.CreateElement("scores");
            doc.AppendChild(rootNode);

            for (int i = 0; i < _scores.Count; i += 1)
            {
                XmlNode scoreNode = doc.CreateElement("score");

                // Name
                XmlNode nameNode = doc.CreateElement("name");
                nameNode.InnerText = _scores[i].name;
                scoreNode.AppendChild(nameNode);

                // Rank
                XmlNode rankNode = doc.CreateElement("rank");
                rankNode.InnerText = (i + 1).ToString();
                scoreNode.AppendChild(rankNode);

                // Value
                XmlNode valueNode = doc.CreateElement("value");
                valueNode.InnerText = _scores[i].score.ToString();
                scoreNode.AppendChild(valueNode);

                rootNode.AppendChild(scoreNode);
            }

            doc.Save(_filepath);
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to save scores at: " + _filepath);
            Console.WriteLine(e);
        }
    }

    public void AddNewRecord(float score, string name = "Anon")
    {
        int insertAt = _scores.Count - 1;
        for (int i = 0; i < _scores.Count; i++)
        {
            if (score > _scores[i].score)
            {
                insertAt = i;
                break;
            }
        }

        _scores.Insert(insertAt, new ScoreData(name, -1, score));
    }

    public void ClearScores()
    {
        _scores.Clear();
    }

    public void LoadScores()
    {
        try
        {
            if (File.Exists(_filepath))
            {
                _scores.Clear();

                XmlDocument doc = new XmlDocument();
                doc.Load(_filepath);

                XmlNodeList elementList = doc.GetElementsByTagName("score");
                foreach (XmlElement e in elementList)
                {
                    string name = e.ChildNodes[0].InnerText;
                    int rank = int.Parse(e.ChildNodes[1].InnerText);
                    float score = float.Parse(e.ChildNodes[2].InnerText);
                    
                    _scores.Add(new ScoreData(name, rank, score));
                }
            }
        }
        catch
        {
            Console.WriteLine("Failed to load scores from: " + _filepath);
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
    public int rank;
    public float score;

    public ScoreData(string name, int rank, float score)
    {
        valid = true;
        this.name = name;
        this.rank = rank;
        this.score = score;
    }

    public override string ToString()
    {
        return ($"{rank}. {name} ({score})");
    }
}
