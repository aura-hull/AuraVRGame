using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenUpdater : MonoBehaviour
{
    [SerializeField] private string powerSuffix = "MW/h";
    [SerializeField] private string timeSuffix = "s";

    [SerializeField] private Text powerProducedDisplay;
    [SerializeField] private Text powerUsedDisplay;
    [SerializeField] private Text netPowerDisplay;
    [SerializeField] private Text timeLeftDisplay;
    [SerializeField] private Text scoreDisplay;

    // Start is called before the first frame update
    void Start()
    {
        // Assign Actions
        ScoreManager.Instance.OnScoreChange += ScoreChanged;
        PowerManager.Instance.OnPowerProducedChanged += PowerProducedChanged;
        PowerManager.Instance.OnPowerUsedChanged += PowerUsedChanged;
        AuraGameManager.Instance.OnPlayDurationChanged += GameTimeChanged;
    }

    private void ScoreChanged(float score)
    {
        scoreDisplay.text = score.ToString();
    }

    private void PowerProducedChanged(float powerProduced)
    {
        powerProducedDisplay.text = Powerify(powerProduced);
        netPowerDisplay.text = $"{Powerify(PowerManager.Instance.NetPower)} ({Percentify(PowerManager.Instance.NetPercentage)})";
    }

    private void PowerUsedChanged(float powerUsed)
    {
        powerUsedDisplay.text = Powerify(powerUsed);
        netPowerDisplay.text = $"{Powerify(PowerManager.Instance.NetPower)} ({Percentify(PowerManager.Instance.NetPercentage)})";
    }

    private void GameTimeChanged(float timeSinceStart, float timeLimit)
    {
        float timeLeft = timeLimit - timeSinceStart;
        timeLeft = Mathf.Round(timeLeft);

        if (timeLeft < 0)
            timeLeft = 0;

        timeLeftDisplay.text = Timeify(timeLeft);
    }

    private string Powerify(float value)
    {
        return $"{value} {powerSuffix}";
    }

    private string Timeify(float value)
    {
        return $"{value} {timeSuffix}";
    }

    private string Percentify(float value)
    {
        return $"{value}%";
    }
}
