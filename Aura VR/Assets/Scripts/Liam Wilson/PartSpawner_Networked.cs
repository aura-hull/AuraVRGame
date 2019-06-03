using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PartSpawner_Networked : MonoBehaviour
{
    [SerializeField] private Object partPrefab;
    [SerializeField] private Transform spawnPoint;

    private bool isSpawned = false;

    public void SpawnIfReady()
    {
        if (isSpawned) return;
        if (partPrefab == null) return;

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(partPrefab.name, spawnPoint.position, Quaternion.identity);
            isSpawned = true;
        }
    }
}
