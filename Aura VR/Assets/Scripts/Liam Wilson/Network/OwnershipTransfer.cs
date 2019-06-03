using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using VRTK;

public class OwnershipTransfer : MonoBehaviour
{
    private PhotonView photonView;
    private VRTK_InteractableObject vrtkObject;

    public void Awake()
    {
        photonView = GetComponent<PhotonView>();

        vrtkObject = GetComponent<VRTK_InteractableObject>();
        vrtkObject.InteractableObjectGrabbed += Transfer;
    }

    private void Transfer(object sender, InteractableObjectEventArgs e)
    {
        photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
    }
}
