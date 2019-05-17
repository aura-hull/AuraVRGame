using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager
{
    private static PowerManager _instance;
    public static PowerManager Instance
    {
        get {
            if (_instance == null) {
                _instance = new PowerManager();
            }
            return _instance;
        }
    }

    private float _powerProduced = 0;
    private float _powerUsed = 0;

    private float PowerProduced
    {
        get
        {
            return _powerProduced;
        }
    }

    private float PowerUsed
    {
        get
        {
            return _powerUsed;
        }
    }
    
    public void IncreasePowerOutput(float amountToIncrease)
    {
        _powerProduced += amountToIncrease;
    }
    public void IncreasePowerUsage(float usage)
    {
        _powerUsed += usage;
    }
}
