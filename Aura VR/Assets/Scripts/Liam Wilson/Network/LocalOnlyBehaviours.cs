using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using VRTK;

public class LocalOnlyBehaviours : MonoBehaviour
{
    [SerializeField] protected VRTK_SDKManager vrtkSdkManager;
    [SerializeField] protected PhotonView photonView;
    [SerializeField] protected VRTK_BodyPhysics bodyPhysics;

    void Awake()
    {
        VRTK_SDKManager.SubscribeLoadedSetupChanged(OnLoadedSetupChanged);
    }

    public void OnLoadedSetupChanged(VRTK_SDKManager sender, VRTK_SDKManager.LoadedSetupChangeEventArgs e)
    {
        if (photonView.IsMine)
        {
            LookAtManager.Instance.lookAtTarget = transform;

            if (bodyPhysics != null)
            {
                bodyPhysics.enabled = true;
            }
        }
    }

    void OnDestroy()
    {
        VRTK_SDKManager.UnsubscribeLoadedSetupChanged(OnLoadedSetupChanged);
    }
}
