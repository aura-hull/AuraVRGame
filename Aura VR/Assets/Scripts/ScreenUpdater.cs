using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenUpdater : MonoBehaviour
{
    [SerializeField] private string netPowerFormat = "{0:f2} MW/s";
    [SerializeField] private string calcPowerFormat = "{0:f2} | {1:f2} MW/s";
    [SerializeField] private string scoreFormat = "{0}";

    [SerializeField] private Text netPowerText;
    [SerializeField] private Text calcPowerText;
    [SerializeField] private Text storedPowerText;
    [SerializeField] private Text timeLeftText;
    [SerializeField] private Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        // Assign Actions
        PowerManager.Instance.OnPowerProducedChanged += OnNetPowerChanged;
        PowerManager.Instance.OnPowerUsedChanged += OnNetPowerChanged;
        OnNetPowerChanged();

        AuraGameManager.Instance.OnPlayDurationChanged += OnGameTimeChanged;
        OnGameTimeChanged();
        
        ScoreManager.Instance.OnScoreChange += OnScoreChanged;
        OnScoreChanged();
    }
    

    private void OnNetPowerChanged()
    {
        float powerNet = PowerManager.Instance.PowerNet;
        netPowerText.text = String.Format(netPowerFormat, powerNet);

        float powerProduced = PowerManager.Instance.PowerProduced;
        float powerUsed = PowerManager.Instance.PowerUsed;
        calcPowerText.text = String.Format(calcPowerFormat, powerProduced, powerUsed);
    }
    
    private void OnGameTimeChanged()
    {
        float timeLeft = AuraGameManager.Instance.TimeRemaining;
        timeLeftText.text = UsefulFuncs.NeatTime(timeLeft);
    }

    private void OnScoreChanged()
    {
        float score = ScoreManager.Instance.Score;
        scoreText.text = String.Format(scoreFormat, score);
    }
}
