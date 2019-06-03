using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PartSpawner : MonoBehaviour
{
    [SerializeField]
    private float _respawnTime;
    [SerializeField]
    private List<GameObject> _spawnObjects;
    [SerializeField]
    private Transform _spawnLocation;
    [SerializeField]
    private string _lookingForTag;

    private int _spawnIndex = 0;
    private PoolManager poolManager;

    private float _timeOfLastPickup;
    private bool _hasSpawnedItem = false;
    public bool IsPowered { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        if (string.IsNullOrEmpty(_lookingForTag))
        {
            //_lookingForTag = "Untagged";
        }

        // Create pools for items
        poolManager = PoolManager.Instance;
        for (int i = 0; i < _spawnObjects.Count; i += 1)
        {
            poolManager.CreatePool(_spawnObjects[i].name, _spawnObjects[i], 5);
        }

        IsPowered = true;
        _timeOfLastPickup = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPowered)
        {
            if (_spawnObjects.Count > 0)
            {
                float timeSincePickup = Time.time - _timeOfLastPickup;
                if (timeSincePickup >= _respawnTime && !_hasSpawnedItem)
                {
                    _hasSpawnedItem = true;
                    // Fetch item from pool
                    GameObject spawnObject = poolManager.SpawnFromPool(_spawnObjects[_spawnIndex].name, _spawnLocation.position, _spawnLocation.localRotation);

                    _spawnIndex++;
                    if (_spawnIndex >= _spawnObjects.Count)
                        _spawnIndex = 0;
                }
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with: " + collision.collider.name);
        if (collision.collider.tag == _lookingForTag && _hasSpawnedItem == true)
        {
            _timeOfLastPickup = Time.time;
            _hasSpawnedItem = false;
        }
    }
}
