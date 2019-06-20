using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

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

    private string _filepath = "ScoreServer/scores.txt";
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
            if (!System.IO.File.Exists(_filepath))
            {
                string path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "ScoreServer");
                System.IO.Directory.CreateDirectory(path);
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

                // DateTime
                XmlNode dateTimeNode = doc.CreateElement("dateTime");
                dateTimeNode.InnerText = _scores[i].time.ToString();
                scoreNode.AppendChild(dateTimeNode);

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

    public void AddNewRecord(float score)
    {
        DateTime time = DateTime.Now;
        ScoreData newScoreData = new ScoreData("Not Set", -1, score, time);

        if (_scores.Count == 0)
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
            else
                _scores.Add(newScoreData);

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
            if (System.IO.File.Exists(_filepath))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(_filepath);

                XmlNodeList elementList = doc.GetElementsByTagName("score");
                foreach (XmlElement e in elementList)
                {
                    string name = e.ChildNodes[0].InnerText;
                    int rank = int.Parse(e.ChildNodes[1].InnerText);
                    float score = float.Parse(e.ChildNodes[2].InnerText);
                    DateTime time = DateTime.Parse(e.ChildNodes[3].InnerText);
                    //DateTime time = DateTime.Now;

                    ScoreData record = new ScoreData(name, rank, score, time);

                    scoresFromFile.Add(record);
                }
            }
        }
        catch
        {
            Console.WriteLine("Failed to load scores from: " + _filepath);
        }

        _scores = scoresFromFile;
    }
}

public struct ScoreData
{
    public bool valid;
    public string name;
    public int rank;
    public float score;
    public DateTime time;

    public ScoreData(string name, int rank, float score, DateTime time)
    {
        valid = true;
        this.name = name;
        this.rank = rank;
        this.score = score;
        this.time = time;
    }

    public override string ToString()
    {
        return (name + "-" + score + "-" + time);
    }
}
