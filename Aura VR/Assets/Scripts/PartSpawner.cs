using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartSpawner : MonoBehaviour
{
    [SerializeField]
    private float _respawnTimer;
    [SerializeField]
    private GameObject _spawnPoint;
    private float _timeSinceLastPickup;
    private PowerState _powerState;
    // Start is called before the first frame update
    void Start()
    {
        _powerState = gameObject.GetComponent<PowerState>();
        _timeSinceLastPickup = _respawnTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (_powerState.ReturnPowerState())
        {
            if (!_spawnPoint.activeSelf)
            {
                _timeSinceLastPickup += Time.deltaTime;
                if (_timeSinceLastPickup > _respawnTimer)
                {
                    _spawnPoint.SetActive(true);
                }
            }
        }
    }
    public void ItemPickedUp()
    {
        _timeSinceLastPickup = 0;
    }
}
