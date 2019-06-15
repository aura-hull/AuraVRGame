using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using VRTK;

public class LocalOnlyBehaviours : MonoBehaviour
{
    [SerializeField] protected VRTK_SDKManager vrtkSdkManager;
    [SerializeField] protected PhotonView photonView;

    void Awake()
    {
        VRTK_SDKManager.SubscribeLoadedSetupChanged(OnLoadedSetupChanged);
    }

    public void OnLoadedSetupChanged(VRTK_SDKManager sender, VRTK_SDKManager.LoadedSetupChangeEventArgs e)
    {
        if (photonView.IsMine)
        {
            LookAtManager.Instance.lookAtTarget = transform;
        }
    }

    void OnDestroy()
    {
        VRTK_SDKManager.UnsubscribeLoadedSetupChanged(OnLoadedSetupChanged);
    }
}
