using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PartSpawner_Networked : MonoBehaviour
{
    [SerializeField] private Object[] partPrefabs;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float distanceBeforeNextSpawn = 10.0f;

    private Transform trackedPart;
    private int nextPartIndex = 0;

    public void SpawnIfReady()
    {
        if (trackedPart != null) return;
        if (partPrefabs == null) return;

        if (PhotonNetwork.IsMasterClient)
        {
            GameObject partInstance = PhotonNetwork.Instantiate(partPrefabs[nextPartIndex].name, spawnPoint.position, Quaternion.identity);
            nextPartIndex = (nextPartIndex + 1) % partPrefabs.Length;
            trackedPart = partInstance.transform;
        }
    }

    void Update()
    {
        if (trackedPart == null) return;

        if (PhotonNetwork.IsMasterClient)
        {
            if (Vector3.Distance(trackedPart.position, spawnPoint.position) > distanceBeforeNextSpawn)
            {
                trackedPart = null;
            }
        }
    }
}
