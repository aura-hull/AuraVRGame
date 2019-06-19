using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class TrackedPhotonInstance : MonoBehaviour
{
    void Start()
    {
        AuraSceneManager.Instance.SubscribeOnSceneReset(this.Destroy);
    }

    public void Destroy()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        PhotonNetwork.Destroy(gameObject.GetPhotonView());
    }

    void OnDestroy()
    {
        AuraSceneManager.Instance.UnsubscribeOnSceneReset(this.Destroy);
    }
}
