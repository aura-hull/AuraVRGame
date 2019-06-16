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
        _consumers = new List<PowerConsumer>();
    }

    private float _powerProduced;
    public Action OnPowerProducedChanged;

    private float _powerUsed;
    public Action OnPowerUsedChanged;

    private float _powerStored;
    public Action OnStoredPowerChanged;

    public WindManager activeWindManager;
    public float depletePowerTime = 1.0f;

    private List<PowerConsumer> _consumers;

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

    private float depletionTimer = 0.0f;
    public void Update()
    {
        float powerUsedCalc = 0.0f;
        foreach (PowerConsumer pc in _consumers)
        {
            powerUsedCalc += pc.ReportConsumption();
        }

        PowerUsed = powerUsedCalc;

        depletionTimer += Time.deltaTime;
        if (depletionTimer >= depletePowerTime)
        {
            PowerStored += PowerNet;
            depletionTimer = 0.0f;
        }
    }

    public void IncreasePowerOutput(float amountToIncrease)
    {
        PowerProduced += amountToIncrease;
    }

    public void SubscribeConsumer(PowerConsumer newConsumer)
    {
        _consumers.Add(newConsumer);
    }

    public void UnsubscribeConsumer(PowerConsumer toRemove)
    {
        if (_consumers.Contains(toRemove))
        {
            _consumers.Remove(toRemove);
        }
    }

    public float CalculatePowerOutput(Vector3 position, Vector3 orientation, float minValue, float maxValue)
    {
        if (activeWindManager == null) return 0.0f;

        float min = activeWindManager.MinKmH;
        float max = activeWindManager.MaxKmH;
        float kmhNormalized = (activeWindManager.GetWindSpeedKmH(position, orientation) - min) / (max - min);

        return Mathf.Lerp(minValue, maxValue, kmhNormalized);
    }
}
