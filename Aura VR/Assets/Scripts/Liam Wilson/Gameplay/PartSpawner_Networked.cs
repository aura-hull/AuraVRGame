using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = System.Random;

public class PartSpawner_Networked : MonoBehaviour
{
    private enum SpawnMode
    {
        Attached,
        Disperse
    }

    [SerializeField] private SpawnMode spawnMode = SpawnMode.Attached;
    [SerializeField] private bool destroyOnSpawn = false;
    [SerializeField] private GameObject[] partPrefabs;
    [SerializeField] private Text timeRemainingText;
    [SerializeField] private float timeBetweenSpawns = 10.0f;
    [SerializeField] private float dispersionRadius = 2.0f;
    [SerializeField] private float requiredCollectorID = -1;

    public Action OnDestroyed;

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
                    timeRemainingText.text = UsefulFuncs.NeatTime(_timeRemaining);

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
        if (timeBetweenSpawns <= 0.0f) SpawnIfReady();
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
        if (partPrefabs == null) return;
        if (_activeCollector == null) return;

        if (!_activeCollector.available) return;
        if (!_partIsReady) return;

        if (PhotonNetwork.IsMasterClient)
        {
            GameObject newPart;

            if (spawnMode == SpawnMode.Attached)
            {
                newPart = PhotonNetwork.Instantiate(partPrefabs[0].name, Vector3.zero, Quaternion.identity);

                newPart.transform.SetParent(_activeCollector.RestingPoint);
                newPart.transform.localPosition = Vector3.zero;
                newPart.transform.localRotation = Quaternion.identity;

                _activeCollector.available = false;

                PartSpawner_Networked newSpawner = newPart.GetComponent<PartSpawner_Networked>();
                if (newSpawner != null)
                {
                    newSpawner.OnDestroyed += _activeCollector.Free;
                }
            }
            else
            {
                for (int i = 0; i < partPrefabs.Length; i++)
                {
                    newPart = PhotonNetwork.Instantiate(partPrefabs[i].name, Vector3.zero, Quaternion.identity);

                    float randomAngle = UnityEngine.Random.Range(0.0f, 360.0f);
                    Vector3 polarPoint = Quaternion.Euler(0, randomAngle, 0) * (Vector3.forward * dispersionRadius);
                    newPart.transform.position = transform.position + polarPoint;
                    newPart.transform.rotation = Quaternion.Euler(randomAngle, randomAngle, randomAngle);
                }
            }
            
            _partIsReady = false;

            if (destroyOnSpawn)
            {
                OnDestroyed?.Invoke();
                PhotonNetwork.Destroy(gameObject.GetPhotonView());
            }
        }
    }
}
