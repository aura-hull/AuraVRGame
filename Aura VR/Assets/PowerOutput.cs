using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerOutput : MonoBehaviour
{
    [SerializeField]
    float powerOutput = 10;
    // Start is called before the first frame update
    void Start()
    {
        PowerManager.instance.IncreasePowerOutput(powerOutput);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
