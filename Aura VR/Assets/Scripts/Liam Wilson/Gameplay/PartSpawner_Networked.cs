using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PartSpawner_Networked : MonoBehaviour
{
    [SerializeField] private Object partPrefab;
    [SerializeField] private Transform spawnPoint;

    private PhotonView currentPart;

    public void SpawnIfReady()
    {
        if (currentPart != null) return;
        if (partPrefab == null) return;

        if (PhotonNetwork.IsMasterClient)
        {
            GameObject partInstance = PhotonNetwork.Instantiate(partPrefab.name, spawnPoint.position, Quaternion.identity);
            currentPart = partInstance.GetPhotonView();
        }
    }
}
