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

    private PowerManager() { }

    private float _powerProduced;
    public Action<float> OnPowerProducedChanged;
    private float _powerUsed;
    public Action<float> OnPowerUsedChanged;

    private float _powerTarget = 100;

    public WindManager activeWindManager;

    public float PowerProduced
    {
        get { return _powerProduced; }
        set
        {
            _powerProduced = value;
            OnPowerProducedChanged?.Invoke(_powerProduced);
        }
    }

    public float PowerUsed
    {
        get { return _powerUsed; }
        set
        {
            _powerUsed = value;
            OnPowerUsedChanged?.Invoke(_powerUsed);
        }
    }

    public float NetPower
    {
        get { return _powerProduced - _powerUsed; }
    }

    public float NetPercentage
    {
        get { return (NetPower / _powerTarget) * 100.0f; }
    }
    
    public void IncreasePowerOutput(float amountToIncrease)
    {
        PowerProduced += amountToIncrease;
    }

    public void IncreasePowerUsage(float usage)
    {
        PowerUsed += usage;
    }

    public float CalculatePowerOutput(Vector3 position, Vector3 orientation)
    {
        if (activeWindManager == null) return 0.0f;
        return activeWindManager.GetWindSpeedKmH(position, orientation);
    }
}
