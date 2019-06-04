using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenUpdater : MonoBehaviour
{
    public TextMeshProUGUI scoreDisplay;
    public TextMeshProUGUI powerUsedDisplay;
    public TextMeshProUGUI powerProducedDisplay;
    public TextMeshProUGUI timeLeftDisplay;
    public Slider netPowerSlider;

    private float _powerProduced = 0;
    private float _powerUsed = 0;
    private float _netPower = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Assign Actions
        ScoreManager.Instance.OnScoreChange += ScoreChanged;
        PowerManager.Instance.OnPowerProducedChanged += PowerProducedChanged;
        PowerManager.Instance.OnPowerUsedChanged += PowerUsedChanged;
        AuraGameManager.Instance.OnPlayDurationChanged += GameTimeChanged;

        scoreDisplay.text = "0";
        powerUsedDisplay.text = "0";
        powerProducedDisplay.text = "0";
        timeLeftDisplay.text = "0";
        netPowerSlider.value = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ScoreChanged(float score)
    {
        scoreDisplay.text = score.ToString();
    }

    private void PowerProducedChanged(float powerProduced)
    {
        _powerProduced = powerProduced;
        _netPower = _powerProduced - _powerUsed;
        powerProducedDisplay.text = _powerProduced.ToString();
    }

    private void PowerUsedChanged(float powerUsed)
    {
        _powerUsed = powerUsed;
        _netPower = _powerProduced - _powerUsed;
        powerUsedDisplay.text = _powerUsed.ToString();
    }

    private void GameTimeChanged(float timeSinceStart, float timeLimit)
    {
        float timeLeft = timeLimit - timeSinceStart;
        timeLeft = Mathf.Round(timeLeft);

        if (timeLeft < 0)
            timeLeft = 0;

        timeLeftDisplay.text = timeLeft.ToString();
    }
}
