using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartSpawner : MonoBehaviour
{
    [SerializeField]
    private float respawnTimer;
    [SerializeField]
    private GameObject spawnPoint;
    private float timeSinceLastPickup;
    private PowerState powerState;
    // Start is called before the first frame update
    void Start()
    {
        powerState = gameObject.GetComponent<PowerState>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawnPoint.activeSelf)
        {
            timeSinceLastPickup += Time.deltaTime;
            if (timeSinceLastPickup > respawnTimer)
            {
                spawnPoint.SetActive(true);
            }            
        }
    }
    public void itemPickedUp()
    {
        timeSinceLastPickup = 0;
    }
}
