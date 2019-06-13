using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class PartSpawner_Networked : MonoBehaviour
{
    [SerializeField] private GameObject partPrefab;
    [SerializeField] private Text timeRemainingText;
    [SerializeField] private float timeBetweenSpawns = 10.0f;
    
    private float _timeRemaining = 0.0f;
    private bool _partIsReady = false;
    private PartCollector _activeCollector = null;

    void Start()
    {
        _timeRemaining = timeBetweenSpawns;
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!_partIsReady)
            {
                _timeRemaining -= Time.deltaTime;

                if (timeRemainingText != null)
                    timeRemainingText.text = NeatTime((int)_timeRemaining);

                if (_timeRemaining <= 0.0f)
                {
                    _partIsReady = true;
                    _timeRemaining = timeBetweenSpawns;

                    if (timeRemainingText != null)
                        timeRemainingText.text = "READY";
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody == null) return;
        _activeCollector = other.attachedRigidbody.GetComponent<PartCollector>();
    }

    void OnTriggerExit(Collider other)
    {
        if (_activeCollector != null && other.attachedRigidbody.gameObject == _activeCollector.gameObject)
        {
            _activeCollector = null;
        }
    }

    public void SpawnIfReady()
    {
        if (partPrefab == null) return;
        if (_activeCollector == null) return;

        if (!_activeCollector.available) return;
        if (!_partIsReady) return;

        if (PhotonNetwork.IsMasterClient)
        {
            GameObject newPart = PhotonNetwork.Instantiate(partPrefab.name, Vector3.zero, Quaternion.identity);
            newPart.transform.SetParent(_activeCollector.RestingPoint, false);

            _activeCollector.available = false;
            _partIsReady = false;
        }
    }

    private string NeatTime(int seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);

        if (seconds < 3600) return time.ToString(@"mm\:ss");
        else return time.ToString(@"hh\:mm\:ss");
    }
}
