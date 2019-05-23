using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartSpawner : MonoBehaviour
{
    [SerializeField]
    private float _respawnTime;
    [SerializeField]
    private GameObject _spawnObject;
    [SerializeField]
    private Transform _spawnLocation;

    private float _timeOfLastPickup;
    public bool IsPowered { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        IsPowered = true;
        _timeOfLastPickup = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPowered)
        {
            float timeSincePickup = Time.time - _timeOfLastPickup;
            if (timeSincePickup >= _respawnTime)
            {
                GameObject newObject = Instantiate<GameObject>(_spawnObject);
                newObject.transform.position = _spawnLocation.position;
                newObject.transform.localRotation = _spawnLocation.localRotation;
                newObject.transform.localScale = _spawnLocation.localScale;
            }
        }
    }
    public void ItemPickedUp()
    {
        _timeOfLastPickup = Time.time;
    }
}
