using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager
{
    #region Singleton
    private static PowerManager _instance;
    public static PowerManager Instance
    {
        get
        {
            if (_instance == null) _instance = new PowerManager();
            return _instance;
        }
    }
    #endregion

    private PowerManager()
    {
        
    }

    private float _powerProduced = 0;
    public Action<float> OnPowerProducedChanged;
    private float _powerUsed = 0;
    public Action<float> OnPowerUsedChanged;

    public float PowerProduced
    {
        get
        {
            return _powerProduced;
        }
        set
        {
            _powerProduced = value;
            OnPowerProducedChanged?.Invoke(_powerProduced);
        }
    }

    public float PowerUsed
    {
        get
        {
            return _powerUsed;
        }
        set
        {
            _powerUsed = value;
            OnPowerUsedChanged?.Invoke(_powerUsed);
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
