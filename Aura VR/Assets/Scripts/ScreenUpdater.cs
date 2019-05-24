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

    // Start is called before the first frame update
    void Start()
    {
        // Assign Actions
        ScoreManager.Instance.OnScoreChange += ScoreChanged;
        PowerManager.Instance.OnPowerProducedChanged += PowerProducedChanged;
        PowerManager.Instance.OnPowerUsedChanged += PowerUsedChanged;
        // Broken, null since GameManager not created yet
        //GameManager.Instance.onPlayDurationChanged += GameTimeChanged;

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
        powerProducedDisplay.text = powerProduced.ToString();
    }

    private void PowerUsedChanged(float powerUsed)
    {
        powerUsedDisplay.text = powerUsed.ToString();
    }

    private void GameTimeChanged(float timeSinceStart, float timeLimit)
    {
        timeLeftDisplay.text = (timeLimit - timeSinceStart).ToString();
    }
}
