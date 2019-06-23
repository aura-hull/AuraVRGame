using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePoint_Networked : MonoBehaviour
{
    [SerializeField] private Text timeRemainingText;
    [SerializeField] private float timeBetweenUpgrades = 60.0f;
    [SerializeField] private float requiredCollectorID = -1;

    private float _timeRemaining = 0.0f;
    private bool _upgradeIsReady = false;
    private PartCollector _activeCollector = null;

    void Start()
    {
        _timeRemaining = timeBetweenUpgrades;
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!_upgradeIsReady)
            {
                _timeRemaining -= Time.deltaTime;

                if (timeRemainingText != null)
                    timeRemainingText.text = UsefulFuncs.NeatTime(_timeRemaining);

                if (_timeRemaining <= 0.0f)
                {
                    _upgradeIsReady = true;
                    _timeRemaining = timeBetweenUpgrades;

                    if (timeRemainingText != null)
                        timeRemainingText.text = "UPGRADES READY";
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        PartCollector collector = other.GetComponent<PartCollector>();

        if (collector == null)
        {
            if (other.attachedRigidbody != null)
            {
                collector = other.attachedRigidbody.GetComponent<PartCollector>();
            }

            if (collector == null) return;
        }

        if (collector.CollectorID == requiredCollectorID)
        {
            _activeCollector = collector;
        }

        // Means this object doesn't use a timer.
        if (timeBetweenUpgrades <= 0.0f) UpgradeIfReady();
    }

    void OnTriggerExit(Collider other)
    {
        if (_activeCollector != null && other.attachedRigidbody.gameObject == _activeCollector.gameObject)
        {
            _activeCollector = null;
        }
    }

    public void UpgradeIfReady()
    {
        if (_activeCollector == null) return;

        if (!_activeCollector.available) return;
        if (!_upgradeIsReady) return;

        if (PhotonNetwork.IsMasterClient)
        {
            UpgradeManager.Instance.Upgrade();

            _timeRemaining = timeBetweenUpgrades;
            _upgradeIsReady = false;
        }
    }
}
