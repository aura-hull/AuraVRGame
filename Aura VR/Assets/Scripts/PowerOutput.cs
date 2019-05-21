using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerOutput : MonoBehaviour
{
    [SerializeField]
    float _powerOutput = 10;
    [SerializeField]
    float _maxDistanceMultiplier = 90;
    // Start is called before the first frame update
    void Start()
    {
        PowerManager.Instance.IncreasePowerOutput(_powerOutput);
        _powerOutput = _powerOutput-(_maxDistanceMultiplier * transform.position.x/150);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
