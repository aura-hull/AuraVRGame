using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager
{
    // Instancing
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
    private PowerManager()
    {

    }

    private float _powerProduced = 0;
    public Action OnPowerProducedChanged;
    private float _powerUsed = 0;
    public Action OnPowerUsedChanged;

    public float PowerProduced
    {
        get
        {
            return _powerProduced;
        }
        private set
        {
            _powerProduced = value;
            OnPowerProducedChanged?.Invoke();
        }
    }

    public float PowerUsed
    {
        get
        {
            return _powerUsed;
        }
        private set
        {
            _powerUsed = value;
            OnPowerUsedChanged?.Invoke();
        }
    }
    
    public void IncreasePowerOutput(float amountToIncrease)
    {
        PowerProduced += amountToIncrease;
    }
    public void IncreasePowerUsage(float usage)
    {
        PowerUsed += usage;
    }
}
