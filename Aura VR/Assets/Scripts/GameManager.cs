using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    private PowerManager _powerManager;
    private ScoreManager _scoreManager;

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
        
    }
}
