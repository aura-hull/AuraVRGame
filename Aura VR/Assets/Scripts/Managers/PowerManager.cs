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
    public Action OnPowerProducedChanged;

    private float _powerUsed;
    public Action OnPowerUsedChanged;

    private float _powerStored;
    public Action OnStoredPowerChanged;

    public WindManager activeWindManager;

    public float PowerProduced
    {
        get { return _powerProduced; }
        set
        {
            _powerProduced = value;
            OnPowerProducedChanged?.Invoke();
        }
    }

    public float PowerUsed
    {
        get { return _powerUsed; }
        set
        {
            _powerUsed = value;
            OnPowerUsedChanged?.Invoke();
        }
    }

    public float PowerNet
    {
        get { return _powerProduced - _powerUsed; }
    }

    public float PowerStored
    {
        get { return _powerStored; }
        set
        {
            _powerStored = value;
            OnStoredPowerChanged?.Invoke();
        }
    }

    public void Update()
    {
        PowerStored += PowerNet;
    }

    public void IncreasePowerOutput(float amountToIncrease)
    {
        PowerProduced += amountToIncrease;
    }

    public void IncreasePowerUsage(float usage)
    {
        PowerUsed += usage;
    }

    public float CalculatePowerOutput(Vector3 position, Vector3 orientation, float maxValue)
    {
        if (activeWindManager == null) return 0.0f;
        return maxValue * (activeWindManager.GetWindSpeedKmH(position, orientation) / activeWindManager.MaxKmH);
    }
}
