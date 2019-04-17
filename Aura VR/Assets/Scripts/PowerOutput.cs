using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerOutput : MonoBehaviour
{
    [SerializeField]
    float _powerOutput = 10;
    // Start is called before the first frame update
    void Start()
    {
        PowerManager.instance.IncreasePowerOutput(_powerOutput);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
