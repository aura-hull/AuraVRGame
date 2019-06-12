using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    [SerializeField] private GameObject boatColliders;
    [SerializeField] private GameObject titanColliders;

    public bool Configure()
    {
        if (boatColliders == null) return false;
        if (titanColliders == null) return false;

        int positionIndex = (int)PhotonNetwork.LocalPlayer.CustomProperties["position"];
        boatColliders.SetActive(positionIndex == 0);
        titanColliders.SetActive(positionIndex == 1);

        return true;
    }
}
