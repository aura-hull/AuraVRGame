using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using AuraHull.AuraVRGame;

public class UpgradeManager
{
    private static float POWER_PRODUCTION_MULTIPLIER = 1.0f;
    private static float POWER_CONSUMPTION_MULTIPLIER = 1.0f;
    private static float MAX_UPGRADE_LEVEL = 5;
    private static float UPGRADE_RATE = 0.2f;
    private static bool DIMINISHING_RETURN = true;

    private static UpgradeManager _instance;
    public static UpgradeManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new UpgradeManager();
            return _instance;
        }
    }

    private float _powerProductionMultiplier = 1.0f;
    private float _powerConsumptionMultiplier = 1.0f;

    private int _upgradeLevel = 0;

    public Action OnUpgradedOrDowngraded;

    public bool upgradesEnabled { get; private set; }

    private UpgradeManager()
    {
        _powerProductionMultiplier = POWER_PRODUCTION_MULTIPLIER;
        _powerConsumptionMultiplier = POWER_CONSUMPTION_MULTIPLIER;
    }

    public float PowerProductionMultiplier
    {
        get { return _powerProductionMultiplier; }
        private set { _powerProductionMultiplier = value; }
    }

    public float PowerConsumptionMultiplier
    {
        get { return _powerConsumptionMultiplier; }
        private set { _powerConsumptionMultiplier = value; }
    }

    public void Upgrade()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (_upgradeLevel == MAX_UPGRADE_LEVEL) return;

        _upgradeLevel++;
        _powerProductionMultiplier += CalculateUpgradeAmount();
        _powerConsumptionMultiplier -= CalculateUpgradeAmount();

        NetworkController.Instance.NotifyUpgradedOrDowngraded(PowerProductionMultiplier, PowerConsumptionMultiplier, _upgradeLevel);
    }

    public void Downgrade()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (_upgradeLevel == 0) return;

        _powerProductionMultiplier -= CalculateUpgradeAmount();
        _powerConsumptionMultiplier += CalculateUpgradeAmount();
        _upgradeLevel--;

        NetworkController.Instance.NotifyUpgradedOrDowngraded(PowerProductionMultiplier, PowerConsumptionMultiplier, _upgradeLevel);
    }

    public void SyncUpgradeState(float powerProductionMultiplier, float powerConsumptionMultiplier, int upgradeLevel)
    {
        PowerProductionMultiplier = powerProductionMultiplier;
        PowerConsumptionMultiplier = powerConsumptionMultiplier;
        _upgradeLevel = upgradeLevel;

        OnUpgradedOrDowngraded?.Invoke();
    }

    private float CalculateUpgradeAmount()
    {
        if (!DIMINISHING_RETURN) return UPGRADE_RATE;
        else return UPGRADE_RATE / _upgradeLevel;
    }

    public void EnableUpgrades()
    {
        upgradesEnabled = true;
    }

    public void DisableUpgrades()
    {
        upgradesEnabled = false;
    }
}
