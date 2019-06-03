using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PartSpawner_Networked : MonoBehaviour
{
    [SerializeField] private Object partPrefab;
    [SerializeField] private Transform spawnPoint;

    void Awake()
    {

    }

    void Start()
    {
        if (partPrefab == null) return;

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(partPrefab.name, spawnPoint.position, Quaternion.identity);
        }
    }

    void Update()
    {

    }
}
