using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    private PowerManager _powerManager;
    private ScoreManager _scoreManager;

    [SerializeField]
    private float _playDurationLimit = 30;
    private float _playDuration = 0;
    private bool _playHasStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        _powerManager = PowerManager.Instance;
        _scoreManager = ScoreManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playHasStarted)
        {
            _playDuration += Time.deltaTime;

            if (_playDuration >= _playDurationLimit)
            {
                // Get final values
                float finalScore = _scoreManager.Score;
                float finalNetPower = _powerManager.PowerProduced - _powerManager.PowerUsed;
                
                // Game should end
            }
        }
    }

    public void StartPlay()
    {
        _playHasStarted = true;
    }
}
