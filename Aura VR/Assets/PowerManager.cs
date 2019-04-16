using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    [SerializeField]
    float powerOutput=0;
    [SerializeField]
    float powerUsage = 0;
    public static PowerManager instance;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void IncreasePowerOutput(float amountToIncrease)
    {
        powerOutput += amountToIncrease;
    }
    public void IncreasePowerUsage(float usage)
    {
        powerUsage += usage;
    }
}
